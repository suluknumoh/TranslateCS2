using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Data.Sqlite;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Inf;




namespace TranslateCS2.Databases;
internal class TranslationsDB : ITranslationsDatabaseService {
    private static readonly string sqliteExtension = $"{AppConfigurationManager.DatabaseExtension}";
    private static readonly string searchPattern = $"*{sqliteExtension}";
    private static readonly string underscore = "_";
    private static readonly string dateTimeFormatString = "yyyy-MM-dd_HH-mm-ss";
    private static readonly uint backUpsToKeep = AppConfigurationManager.DatabaseMaxBackUpCount;
    private static string ConnectionString { get; } = AppConfigurationManager.DatabaseConnectionString;

    public void EnrichTranslationSessions(ITranslationSessionManager translationSessionManager, ITranslationsDatabaseService.OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = this.GetOpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText =
@"
    SELECT
        id,
        started,
        last_edited,
        name,
        merge_loc_file,
        overwrite_loc_file,
        localizationnameen,
        localizationnamelocalized,
        autosave
    FROM
        t_translation_session
    ORDER BY
        id ASC;";
            command.CommandType = CommandType.Text;
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                ITranslationSession session = translationSessionManager.GetNewTranslationSession();
                ReadOnlyCollection<DbColumn> columnSchema = reader.GetColumnSchema();
                foreach (DbColumn column in columnSchema) {
                    if (column.ColumnOrdinal is null) {
                        continue;
                    }
                    int ordinal = (int) column.ColumnOrdinal;
                    switch (column.ColumnName) {
                        case "id":
                            session.ID = reader.GetInt64(ordinal);
                            break;
                        case "started":
                            session.Started = reader.GetDateTime(ordinal).ToLocalTime();
                            break;
                        case "last_edited":
                            session.LastEdited = reader.GetDateTime(ordinal).ToLocalTime();
                            break;
                        case "name":
                            session.Name = reader.GetString(ordinal);
                            break;
                        case "merge_loc_file":
                            if (!reader.IsDBNull(ordinal)) {
                                session.MergeLocalizationFileName = reader.GetString(ordinal);
                            }
                            break;
                        case "overwrite_loc_file":
                            // gibts nicht mehr
                            break;
                        case "localizationnameen":
                            session.LocNameEnglish = reader.GetString(ordinal);
                            break;
                        case "localizationnamelocalized":
                            session.LocName = reader.GetString(ordinal);
                            break;
                        case "autosave":
                            // INFO: is always true
                            //session.IsAutoSave = reader.GetBoolean(ordinal);
                            break;
                    }
                }
                translationSessionManager.TranslationSessions.Add(session);

            }
            if (translationSessionManager.TranslationSessions.Any()) {
                translationSessionManager.CurrentTranslationSession = translationSessionManager.TranslationSessions.Last();
            }
        } catch {
            onError?.Invoke(I18NGlobal.MessageDatabaseError);
        }
    }

    public void UpsertTranslationSession(ITranslationSession translationSession, ITranslationsDatabaseService.OnErrorCallBack? onError) {
        try {
            if (translationSession.ID > 0) {
                translationSession.LastEdited = DateTime.Now;
            }
            using SqliteConnection connection = this.GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                using SqliteCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
@"
    INSERT
        INTO
            t_translation_session
            (
                id,
                started,
                last_edited,
                name,
                merge_loc_file,
                overwrite_loc_file,
                localizationnameen,
                localizationnamelocalized,
                autosave
            )
        VALUES
            (
                @id,
                @started,
                @last_edited,
                @name,
                @merge_loc_file,
                @overwrite_loc_file,
                @localizationnameen,
                @localizationnamelocalized,
                @autosave
            )
    ON CONFLICT
        (
            id
        )
    DO UPDATE SET
        last_edited = excluded.last_edited,
        name = excluded.name,
        merge_loc_file = excluded.merge_loc_file,
        overwrite_loc_file = excluded.overwrite_loc_file,
        localizationnameen = excluded.localizationnameen,
        localizationnamelocalized = excluded.localizationnamelocalized;";
                command.CommandType = CommandType.Text;
                SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                SqliteParameter nameParameter = command.Parameters.Add("@name", SqliteType.Text);
                SqliteParameter startedParameter = new SqliteParameter {
                    ParameterName = "@started"
                };
                startedParameter = command.Parameters.Add(startedParameter);
                SqliteParameter lastEditedParameter = new SqliteParameter {
                    ParameterName = "@last_edited"
                };
                lastEditedParameter = command.Parameters.Add(lastEditedParameter);
                SqliteParameter mergeLocFileParameter = command.Parameters.Add("@merge_loc_file", SqliteType.Text);
                SqliteParameter overwriteFileParameter = command.Parameters.Add("@overwrite_loc_file", SqliteType.Text);
                SqliteParameter localizationNameEnParameter = command.Parameters.Add("@localizationnameen", SqliteType.Text);
                SqliteParameter localizationNameLocalizedParameter = command.Parameters.Add("@localizationnamelocalized", SqliteType.Text);
                SqliteParameter autosaveParameter = command.Parameters.Add("@autosave", SqliteType.Integer);
                command.Prepare();
                if (translationSession.ID == 0) {
                    idParameter.Value = DBNull.Value;
                } else {
                    idParameter.Value = translationSession.ID;
                }
                nameParameter.Value = translationSession.Name?.Trim();
                startedParameter.Value = translationSession.Started.ToUniversalTime();
                lastEditedParameter.Value = translationSession.LastEdited.ToUniversalTime();
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(translationSession.MergeLocalizationFileName)) {
                    mergeLocFileParameter.Value = DBNull.Value;
                } else {
                    mergeLocFileParameter.Value = translationSession.MergeLocalizationFileName;
                }
                // must not be null
                overwriteFileParameter.Value = "DBNull";
                localizationNameEnParameter.Value = translationSession.LocNameEnglish;
                localizationNameLocalizedParameter.Value = translationSession.LocName;
                autosaveParameter.Value = translationSession.IsAutoSave ? 1 : 0;
                command.ExecuteNonQuery();
                if (translationSession.ID == 0) {
                    command.CommandText = "SELECT last_insert_rowid();";
                    object? ret = command.ExecuteScalar();
                    if (ret is long l) {
                        translationSession.ID = l;
                    } else {
                        ArgumentNullException.ThrowIfNull(ret);
                    }
                }
                transaction.Commit();
            } catch {
                transaction.Rollback();
                throw;
            }
        } catch {
            onError?.Invoke(I18NGlobal.MessageDatabaseError);
        }
    }

    public void EnrichSavedTranslations(ITranslationSession session, ITranslationsDatabaseService.OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = this.GetOpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText =
@"
    SELECT
        key,
        translation
    FROM
        t_translation
    WHERE
        id_translation_session = @id;";
            command.CommandType = CommandType.Text;
            SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
            command.Prepare();
            idParameter.Value = session.ID;
            using SqliteDataReader reader = command.ExecuteReader();
            Dictionary<string, string> valueTranslationMapping = [];
            while (reader.Read()) {
                if (reader.IsDBNull(0) || reader.IsDBNull(1)) {
                    continue;
                }
                string key = reader.GetString(0);
                string translation = reader.GetString(1);
                this.EnrichSavedTranslation(session.Localizations, key, translation, valueTranslationMapping);
            }
            this.EnrichNewWithTranslatedValue(session, valueTranslationMapping, onError);
        } catch {
            onError?.Invoke(I18NGlobal.MessageDatabaseError);
        }
    }

    /// <summary>
    ///     checks, if there are new entries with an already translated value and reflects the already translated value into those entries
    ///     </br>
    ///     </br>
    ///     is done in c# code, cause i feel safer having the key within the database...
    /// </summary>
    /// <param name="session">
    ///     <see cref="ITranslationSession"/>
    /// </param>
    /// <param name="valueTranslationMapping">
    ///     <see cref="Dictionary{string, string}"/> where TKey is the english value and TValue is the current translation
    /// </param>
    private void EnrichNewWithTranslatedValue(ITranslationSession session,
                                              Dictionary<string, string> valueTranslationMapping,
                                              ITranslationsDatabaseService.OnErrorCallBack? onError) {
        IEnumerable<KeyValuePair<string, IAppLocFileEntry>> newEntries =
            session.Localizations.Where(
                entry => !StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value.Value)
                         && valueTranslationMapping.ContainsKey(entry.Value.Value)
                         && StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value.Translation)
        );
        if (newEntries.Any()) {
            foreach (KeyValuePair<string, IAppLocFileEntry> entry in newEntries) {
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value.Value)) {
                    continue;
                }
                bool got = valueTranslationMapping.TryGetValue(entry.Value.Value, out string? translation);
                if (got
                    && !StringHelper.IsNullOrWhiteSpaceOrEmpty(translation)) {
                    entry.Value.Translation = translation;
                }
            }
            this.SaveTranslations(session, onError);
        }
    }

    private void EnrichSavedTranslation(ICollection<KeyValuePair<string, IAppLocFileEntry>> localizationDictionary,
                                        string key,
                                        string translation,
                                        Dictionary<string, string> valueTranslationMapping) {
        foreach (KeyValuePair<string, IAppLocFileEntry> entry in localizationDictionary) {
            if (entry.Key == key) {
                entry.Value.Translation = translation;
                if (!StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Value.Value)) {
                    valueTranslationMapping.TryAdd(entry.Value.Value, entry.Value.Translation);
                }
                return;
            }
        }
        // manually added key-translation-pair
        IAppLocFileEntry add = AppLocFileEntryFactory.Create(key, null, null, translation, true);
        localizationDictionary.Add(new KeyValuePair<string, IAppLocFileEntry>(add.Key.Key, add));
    }

    public void SaveTranslations(ITranslationSession translationSession, ITranslationsDatabaseService.OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = this.GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                this.UpdateLastEdited(connection, transaction, translationSession);
                IEnumerable<KeyValuePair<string, IAppLocFileEntry>> deletes =
                    translationSession
                        .Localizations
                            .Where(item => StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value.Translation));
                if (deletes.Any()) {
                    using SqliteCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText =
@"
    DELETE
        FROM
            t_translation
        WHERE
            id_translation_session = @id
            AND key = @key;";
                    command.CommandType = CommandType.Text;
                    SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                    SqliteParameter keyParameter = command.Parameters.Add("@key", SqliteType.Text);
                    command.Prepare();
                    foreach (KeyValuePair<string, IAppLocFileEntry> delete in deletes) {
                        idParameter.Value = translationSession.ID;
                        keyParameter.Value = delete.Key;
                        command.ExecuteNonQuery();
                    }
                }
                IEnumerable<KeyValuePair<string, IAppLocFileEntry>> upserts =
                        translationSession
                            .Localizations
                                .Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Value.Translation));
                if (upserts.Any()) {
                    using SqliteCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText =
@"
    INSERT
        INTO
            t_translation
            (
                id_translation_session,
                key,
                translation
            )
        VALUES
            (
                @id,
                @key,
                @translation
            )
        ON CONFLICT
            (
                id_translation_session,
                key
            )
        DO UPDATE SET
            translation = excluded.translation;";
                    command.CommandType = CommandType.Text;
                    SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                    SqliteParameter keyParameter = command.Parameters.Add("@key", SqliteType.Text);
                    SqliteParameter translationParameter = command.Parameters.Add("@translation", SqliteType.Text);
                    command.Prepare();
                    foreach (KeyValuePair<string, IAppLocFileEntry> upsert in upserts) {
                        idParameter.Value = translationSession.ID;
                        keyParameter.Value = upsert.Key;
                        translationParameter.Value = upsert.Value.Translation;
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            } catch {
                transaction.Rollback();
                throw;
            }
        } catch {
            onError?.Invoke(I18NGlobal.MessageDatabaseError);
        }
    }

    private void UpdateLastEdited(SqliteConnection connection, SqliteTransaction transaction, ITranslationSession translationSession) {
        translationSession.LastEdited = DateTime.Now;
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
@"
    UPDATE
        t_translation_session
    SET
        last_edited = @last_edited
    WHERE
        id = @id;";
        command.CommandType = CommandType.Text;
        SqliteParameter lastEditedParameter = new SqliteParameter {
            ParameterName = "@last_edited"
        };
        lastEditedParameter = command.Parameters.Add(lastEditedParameter);
        SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
        command.Prepare();
        lastEditedParameter.Value = translationSession.LastEdited.ToUniversalTime();
        idParameter.Value = translationSession.ID;
        command.ExecuteNonQuery();
    }

    private SqliteConnection GetOpenConnection() {
        if (!this.DatabaseExists()) {
            throw new ArgumentNullException();
        }

        // no using!!!
        SqliteConnection connection = new SqliteConnection(ConnectionString);
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        connection.Open();
        return connection;
    }

    public void DeleteTranslationSession(ITranslationSession translationSession, ITranslationsDatabaseService.OnErrorCallBack onError) {
        try {
            using SqliteConnection connection = this.GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                using SqliteCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText =
@"
    DELETE
        FROM
            t_translation_session
        WHERE
            id = @id;";
                command.CommandType = CommandType.Text;
                SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                command.Prepare();
                idParameter.Value = translationSession.ID;
                command.ExecuteNonQuery();
                transaction.Commit();
            } catch {
                transaction.Rollback();
                throw;
            }
        } catch {
            onError?.Invoke(I18NGlobal.MessageDatabaseError);
        }
    }
    public void CreateIfNotExists() {
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        if (!this.DatabaseExists()) {
            string databaseName = AppConfigurationManager.DatabaseName;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? databaseAssetStream = assembly.GetManifestResourceStream(assetPath + databaseName);
            ArgumentNullException.ThrowIfNull(databaseAssetStream, nameof(databaseAssetStream));
            using Stream output = File.OpenWrite(databaseName);
            byte[] buffer = new byte[1024];
            while (databaseAssetStream.Read(buffer) > 0) {
                output.Write(buffer);
            }
        }
    }

    /// <seealso href="https://stackoverflow.com/questions/71986869/disable-sqlite-automatic-database-creation-c-sharp"/>
    public bool DatabaseExists() {
        return File.Exists(AppConfigurationManager.DatabaseName);
    }

    public void BackUpIfExists(DatabaseBackUpIndicators backUpIndicator) {
        if (backUpsToKeep == 0) {
            return;
        }
        string dateTimeString = DateTime.Now.ToString(dateTimeFormatString);
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        if (this.DatabaseExists()) {
            string backUpFileName = String.Concat(AppConfigurationManager.DatabaseNameRaw,
                                                    underscore,
                                                    dateTimeString,
                                                    underscore,
                                                    backUpIndicator.ToString(),
                                                    sqliteExtension);
            string databaseName = AppConfigurationManager.DatabaseName;
            File.Copy(databaseName, backUpFileName);
            this.RemoveEldestBackUps(databaseName);
        }
    }

    private void RemoveEldestBackUps(string databaseName) {
        string directory = Directory.GetCurrentDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        IEnumerable<FileInfo> backUps = directoryInfo.EnumerateFiles(searchPattern).Where(f => f.Name != databaseName);
        IOrderedEnumerable<FileInfo> sqlitesOrdered = backUps.OrderByDescending(f => f.CreationTimeUtc);
        IEnumerable<FileInfo> deletes = sqlitesOrdered.Skip((int) backUpsToKeep);
        foreach (FileInfo delete in deletes) {
            try {
                delete.Delete();
            } catch {
                // does not matter
            }
        }
    }
}
