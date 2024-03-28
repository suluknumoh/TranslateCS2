using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Common;
using System.Linq;

using Microsoft.Data.Sqlite;

using TranslateCS2.Helpers;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties;



namespace TranslateCS2.Databases;
internal static class TranslationsDB {
    private static readonly string _databaseName = ConfigurationManager.AppSettings["DatabaseNameTranslations"];
    private static readonly ConnectionStringSettings _connectionStringSettings = ConfigurationManager.ConnectionStrings[_databaseName];

    public delegate void OnErrorCallBack(string message);

    public static void EnrichTranslationSessions(TranslationSessionManager translationSessionManager, OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id, started, last_edited, name, merge_loc_file, overwrite_loc_file, localizationnameen, localizationnamelocalized, autosave FROM t_translation_session ORDER BY id ASC;";
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
                            session.Started = reader.GetDateTime(ordinal);
                            break;
                        case "last_edited":
                            session.LastEdited = reader.GetDateTime(ordinal);
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
            onError?.Invoke(I18N.MessageErrorTranslationsDB);
        }
    }

    public static void AddTranslationSession(TranslationSession translationSession, OnErrorCallBack? onError) {
        try {
            translationSession.Started = DateTime.UtcNow;
            translationSession.LastEdited = translationSession.Started;
            using SqliteConnection connection = GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                using SqliteCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO t_translation_session (id, started, last_edited, name, merge_loc_file, overwrite_loc_file, localizationnameen, localizationnamelocalized, autosave) VALUES (null, @started, @last_edited, @name, @merge_loc_file, @overwrite_loc_file, @localizationnameen, @localizationnamelocalized, @autosave);";
                command.CommandType = System.Data.CommandType.Text;
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
                nameParameter.Value = translationSession.Name?.Trim();
                startedParameter.Value = translationSession.Started;
                lastEditedParameter.Value = translationSession.LastEdited;
                if (String.IsNullOrEmpty(translationSession.MergeLocalizationFileName) || String.IsNullOrWhiteSpace(translationSession.MergeLocalizationFileName)) {
                    mergeLocFileParameter.Value = DBNull.Value;
                } else {
                    mergeLocFileParameter.Value = translationSession.MergeLocalizationFileName;
                }
                overwriteFileParameter.Value = translationSession.OverwriteLocalizationFileName;
                localizationNameEnParameter.Value = translationSession.OverwriteLocalizationNameEN;
                localizationNameLocalizedParameter.Value = translationSession.OverwriteLocalizationNameLocalized;
                autosaveParameter.Value = translationSession.IsAutoSave ? 1 : 0;
                command.ExecuteNonQuery();
                command.CommandText = "SELECT last_insert_rowid();";
                translationSession.ID = (long) command.ExecuteScalar();
                transaction.Commit();
            } catch {
                transaction.Rollback();
                throw;
            }
        } catch {
            onError?.Invoke(I18N.MessageErrorTranslationsDB);
        }
    }

    public static void UpdateTranslationSession(TranslationSession translationSession, OnErrorCallBack? onError) {
        try {
            translationSession.LastEdited = DateTime.UtcNow;
            using SqliteConnection connection = GetOpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            try {
                using SqliteCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "UPDATE t_translation_session SET last_edited = @last_edited, name = @name, merge_loc_file = @merge_loc_file, overwrite_loc_file = @overwrite_loc_file, localizationnameen = @localizationnameen, localizationnamelocalized = @localizationnamelocalized, autosave = @autosave WHERE id = @id;";
                command.CommandType = System.Data.CommandType.Text;
                SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                SqliteParameter lastEditedParameter = new SqliteParameter {
                    ParameterName = "@last_edited"
                };
                lastEditedParameter = command.Parameters.Add(lastEditedParameter);
                SqliteParameter nameParameter = command.Parameters.Add("@name", SqliteType.Text);
                SqliteParameter mergeLocFileParameter = command.Parameters.Add("@merge_loc_file", SqliteType.Text);
                SqliteParameter overwriteFileParameter = command.Parameters.Add("@overwrite_loc_file", SqliteType.Text);
                SqliteParameter localizationNameEnParameter = command.Parameters.Add("@localizationnameen", SqliteType.Text);
                SqliteParameter localizationNameLocalizedParameter = command.Parameters.Add("@localizationnamelocalized", SqliteType.Text);
                SqliteParameter autosaveParameter = command.Parameters.Add("@autosave", SqliteType.Integer);
                command.Prepare();
                idParameter.Value = translationSession.ID;
                lastEditedParameter.Value = translationSession.LastEdited;
                nameParameter.Value = translationSession.Name?.Trim();
                if (String.IsNullOrEmpty(translationSession.MergeLocalizationFileName) || String.IsNullOrWhiteSpace(translationSession.MergeLocalizationFileName)) {
                    mergeLocFileParameter.Value = DBNull.Value;
                } else {
                    mergeLocFileParameter.Value = translationSession.MergeLocalizationFileName;
                }
                overwriteFileParameter.Value = translationSession.OverwriteLocalizationFileName;
                localizationNameEnParameter.Value = translationSession.OverwriteLocalizationNameEN;
                localizationNameLocalizedParameter.Value = translationSession.OverwriteLocalizationNameLocalized;
                autosaveParameter.Value = translationSession.IsAutoSave ? 1 : 0;
                command.ExecuteNonQuery();
                transaction.Commit();
            } catch {
                transaction.Rollback();
                throw;
            }
        } catch {
            onError?.Invoke(I18N.MessageErrorTranslationsDB);

        }
    }

    public static void EnrichSavedTranslations(TranslationSession session, OnErrorCallBack? onError) {
        try {
            using SqliteConnection connection = GetOpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT key, translation FROM t_translation WHERE id_translation_session = {session.ID};";
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
            onError?.Invoke(I18N.MessageErrorTranslationsDB);
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
        IEnumerable<LocalizationDictionaryEntry> newEntries = session.LocalizationDictionary.Where(entry => valueTranslationMapping.ContainsKey(entry.Value) && (String.IsNullOrEmpty(entry.Translation) || String.IsNullOrWhiteSpace(entry.Translation)));
        if (newEntries.Any()) {
            foreach (LocalizationDictionaryEntry entry in newEntries) {
                bool got = valueTranslationMapping.TryGetValue(entry.Value, out string? translation);
                if (got && !String.IsNullOrEmpty(translation) && !String.IsNullOrWhiteSpace(translation)) {
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
                UpdateLastUpdated(connection, transaction, translationSession);
                IEnumerable<LocalizationDictionaryEntry> deletes = translationSession.LocalizationDictionary.Where(item => String.IsNullOrEmpty(item.Translation) || String.IsNullOrWhiteSpace(item.Translation));
                if (deletes.Any()) {
                    using SqliteCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "DELETE FROM t_translation WHERE id_translation_session = @id AND key = @key;";
                    command.CommandType = System.Data.CommandType.Text;
                    SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
                    SqliteParameter keyParameter = command.Parameters.Add("@key", SqliteType.Text);
                    command.Prepare();
                    foreach (LocalizationDictionaryEntry delete in deletes) {
                        idParameter.Value = translationSession.ID;
                        keyParameter.Value = delete.Key;
                        command.ExecuteNonQuery();
                    }
                }
                IEnumerable<LocalizationDictionaryEntry> upserts = translationSession.LocalizationDictionary.Where(item => !String.IsNullOrEmpty(item.Translation) && !String.IsNullOrWhiteSpace(item.Translation));
                if (upserts.Any()) {
                    using SqliteCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO t_translation (id_translation_session, key, translation) VALUES (@id, @key, @translation) ON CONFLICT (id_translation_session, key) DO UPDATE SET translation = excluded.translation;";
                    command.CommandType = System.Data.CommandType.Text;
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
            onError?.Invoke(I18N.MessageErrorTranslationsDB);
        }
    }

    private static void UpdateLastUpdated(SqliteConnection connection, SqliteTransaction transaction, TranslationSession translationSession) {
        translationSession.LastEdited = DateTime.UtcNow;
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "UPDATE t_translation_session SET last_edited = @last_edited WHERE id = @id;";
        command.CommandType = System.Data.CommandType.Text;
        SqliteParameter lastEditedParameter = new SqliteParameter {
            ParameterName = "@last_edited"
        };
        lastEditedParameter = command.Parameters.Add(lastEditedParameter);
        SqliteParameter idParameter = command.Parameters.Add("@id", SqliteType.Integer);
        command.Prepare();
        lastEditedParameter.Value = translationSession.LastEdited;
        idParameter.Value = translationSession.ID;
        command.ExecuteNonQuery();
    }

    private static SqliteConnection GetOpenConnection() {
        if (!DatabaseHelper.DatabaseExists(_connectionStringSettings)) {
            throw new ArgumentNullException();
        }

        // no using!!!
        SqliteConnection connection = new SqliteConnection(_connectionStringSettings.ConnectionString);
        connection.Open();
        return connection;
    }
}
