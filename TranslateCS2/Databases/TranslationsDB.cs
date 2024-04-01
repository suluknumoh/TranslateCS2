using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Data.Sqlite;

using TranslateCS2.Configurations;
using TranslateCS2.Helpers;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;



namespace TranslateCS2.Databases;
internal static class TranslationsDB {
    private static string ConnectionString { get; } = AppConfigurationManager.DatabaseConnectionString;

    public delegate void OnErrorCallBack(string message);

    public static void EnrichTranslationSessions(TranslationSessionManager translationSessionManager, OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
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
                TranslationSession session = new TranslationSession();
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
                            session.OverwriteLocalizationFileName = reader.GetString(ordinal);
                            break;
                        case "localizationnameen":
                            session.OverwriteLocalizationNameEN = reader.GetString(ordinal);
                            break;
                        case "localizationnamelocalized":
                            session.OverwriteLocalizationNameLocalized = reader.GetString(ordinal);
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

    public static void UpsertTranslationSession(TranslationSession translationSession, OnErrorCallBack? onError) {
        try {
            if (translationSession.ID > 0) {
                translationSession.LastEdited = DateTime.Now;
            }
            using SqliteConnection connection = GetOpenConnection();
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
                overwriteFileParameter.Value = translationSession.OverwriteLocalizationFileName;
                localizationNameEnParameter.Value = translationSession.OverwriteLocalizationNameEN;
                localizationNameLocalizedParameter.Value = translationSession.OverwriteLocalizationNameLocalized;
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

    public static void EnrichSavedTranslations(TranslationSession session, OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
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
                EnrichSavedTranslation(session.LocalizationDictionary, key, translation, valueTranslationMapping);
            }
            EnrichNewWithTranslatedValue(session, valueTranslationMapping, onError);
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
    ///     <see cref="TranslationSession"/>
    /// </param>
    /// <param name="valueTranslationMapping">
    ///     <see cref="Dictionary{string, string}"/> where TKey is the english value and TValue is the current translation
    /// </param>
    private static void EnrichNewWithTranslatedValue(TranslationSession session, Dictionary<string, string> valueTranslationMapping, OnErrorCallBack? onError) {
        IEnumerable<LocalizationDictionaryEntry> newEntries =
            session.LocalizationDictionary.Where(
                entry => valueTranslationMapping.ContainsKey(entry.Value) && StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Translation)
        );
        if (newEntries.Any()) {
            foreach (LocalizationDictionaryEntry entry in newEntries) {
                bool got = valueTranslationMapping.TryGetValue(entry.Value, out string? translation);
                if (got && !StringHelper.IsNullOrWhiteSpaceOrEmpty(translation)) {
                    entry.Translation = translation;
                }
            }
            SaveTranslations(session, onError);
        }
    }

    private static void EnrichSavedTranslation(ObservableCollection<LocalizationDictionaryEntry> localizationDictionary, string key, string translation, Dictionary<string, string> valueTranslationMapping) {
        foreach (LocalizationDictionaryEntry entry in localizationDictionary) {
            if (entry.Key == key) {
                entry.Translation = translation;
                valueTranslationMapping.TryAdd(entry.Value, entry.Translation);
                break;
            }
        }
    }

    public static void SaveTranslations(TranslationSession translationSession, OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                UpdateLastEdited(connection, transaction, translationSession);
                IEnumerable<LocalizationDictionaryEntry> deletes = translationSession.LocalizationDictionary.Where(item => StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation));
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
                    foreach (LocalizationDictionaryEntry delete in deletes) {
                        idParameter.Value = translationSession.ID;
                        keyParameter.Value = delete.Key;
                        command.ExecuteNonQuery();
                    }
                }
                IEnumerable<LocalizationDictionaryEntry> upserts = translationSession.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation));
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
                    foreach (LocalizationDictionaryEntry upsert in upserts) {
                        idParameter.Value = translationSession.ID;
                        keyParameter.Value = upsert.Key;
                        translationParameter.Value = upsert.Translation;
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

    private static void UpdateLastEdited(SqliteConnection connection, SqliteTransaction transaction, TranslationSession translationSession) {
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

    private static SqliteConnection GetOpenConnection() {
        if (!DatabaseHelper.DatabaseExists()) {
            throw new ArgumentNullException();
        }

        // no using!!!
        SqliteConnection connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public static void DeleteTranslationSession(TranslationSession translationSession, OnErrorCallBack onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
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
}
