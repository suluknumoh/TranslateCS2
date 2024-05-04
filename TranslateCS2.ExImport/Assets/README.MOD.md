# TranslateCS2.Mod

This mod is intended to sideload custom localizations/translations from JSONs.

# Disclaimer
This mod is provided as is to use at your own risk.

Please see also: [MIT-License](https://github.com/suluknumoh/TranslateCS2?tab=MIT-1-ov-file)

# JSON-File-Format

* a .json-file
* should contain a single Object
* this Object should have commaseparated Key-Value-Pairs
* the Key should be the Key that is used by Colossal Order
* the Value should be the translation
* should be UTF8-encoded
* example:


{
"TranslateCS2.LocaleNameLocalizedKey": "Nederlands",
"Options.SECTION[General]": "Algemeen"
}


* the example above contains the Key-Value-Pair "TranslateCS2.LocaleNameLocalizedKey": "Nederlands"
* that Key is not needed and may be left
* if that Key-Value-Pair is missing, the native name is gathered from the supported CultureInfo that matches the filename and is shown within the drop down to select a flavor
* if the Key-Value-Pair is present and its Value is not left empty, its Value is shown within the drop down to select a flavor

* the example above contains the Key "Options.SECTION[General]": "Algemeen"
* please take a look at 'working-hierarchy' within this document

# JSON-File-Naming and Location
* example: name it 'af-ZA.json' for 'Afrikaans (Suid-Afrika)'
* a list of supported language-/language-region-code can be found in the paragraph 'Supported language and language-region-codes with their English names' within this document
* the .json-file should be placed into the following directory:
%AppData%\\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\

# working-hierarchy
As long as no other mod is loaded after this one and overwrites the values and as long as no other mod overwrites the corresponding methods to return other values

## built-in languages flavor 'none' selected
* built-in Values/Texts/Translations are used

## built-in languages and a provided flavor is selected
* if a 'new' Value/Text/Translation is available within the specific provided translation-file, that Value/Text/Translation is used
* otherwise the Value/Text/Translation provided by Colossal Order is used


## new non built-in languages
* only languages, a respective language file is provided, are available within the language drop down in the default interface-settings
* after selecting such a new non built-in language within the default interface-settings, the first found flavor is applied
* if there is only one flavor provided, everything should be fine
* if there are multiple flavors provided, the desired ones can be selected within the 'TRANSLATECS2'-Menu
* flavor drop downs do not contain 'none' as selectable item


* if a new Value/Text/Translation is available for a specific Key within the specific provided and selected translation-file (flavor), that Value/Text/Translation is used
* otherwise Colossal Orders Value/Text/Translation for that specific Key is taken from Colossal Orders fallback-language ('en-US', as far as i know/gathered) and used


# more how it works explanation
* this mod knows your previous selected language
* this mod also knows your selected language while this mod is active
* this mod also knows your selected flavor while this mod is active
* at the latest when the game is left those settings are saved
* as soon as the game is exited, the previous selected language or, if unknown for whatever reason, the language of the used operating system is set to the default interface-settings to grant specific game experience if the game is started without this mod next time


# Supported language and language-region-codes with their English names
* the following languages are supported with the listed country and/or country-region code
* due to technical limitations, country and/or country-region codes are limited

## Afrikaans
* af - Afrikaans
* af-ZA - Afrikaans (South Africa)


## Arabic
* ar - Arabic
* ar-AE - Arabic (United Arab Emirates)
* ar-BH - Arabic (Bahrain)
* ar-DZ - Arabic (Algeria)
* ar-EG - Arabic (Egypt)
* ar-IQ - Arabic (Iraq)
* ar-JO - Arabic (Jordan)
* ar-KW - Arabic (Kuwait)
* ar-LB - Arabic (Lebanon)
* ar-LY - Arabic (Libya)
* ar-MA - Arabic (Morocco)
* ar-OM - Arabic (Oman)
* ar-QA - Arabic (Qatar)
* ar-SA - Arabic (Saudi Arabia)
* ar-SY - Arabic (Syria)
* ar-TN - Arabic (Tunisia)
* ar-YE - Arabic (Yemen)


## Basque
* eu - Basque
* eu-ES - Basque (Basque)


## Belarusian
* be - Belarusian
* be-BY - Belarusian (Belarus)


## Bulgarian
* bg - Bulgarian
* bg-BG - Bulgarian (Bulgaria)


## Catalan
* ca - Catalan
* ca-ES - Catalan (Catalan)


## ChineseSimplified
* zh-CN - Chinese (Simplified, China)
* zh-Hans - Chinese (Simplified)
* zh-SG - Chinese (Simplified, Singapore)
* zh-CHS - Chinese (Simplified) Legacy


## ChineseTraditional
* zh-Hant - Chinese (Traditional)
* zh-HK - Chinese (Traditional, Hong Kong SAR)
* zh-MO - Chinese (Traditional, Macao SAR)
* zh-TW - Chinese (Traditional, Taiwan)
* zh-CHT - Chinese (Traditional) Legacy


## Czech
* cs - Czech
* cs-CZ - Czech (Czechia)


## Danish
* da - Danish
* da-DK - Danish (Denmark)


## Dutch
* nl - Dutch
* nl-BE - Dutch (Belgium)
* nl-NL - Dutch (Netherlands)


## English
* en - English
* en-AU - English (Australia)
* en-BZ - English (Belize)
* en-CA - English (Canada)
* en-GB - English (United Kingdom)
* en-HK - English (Hong Kong SAR)
* en-IE - English (Ireland)
* en-IN - English (India)
* en-JM - English (Jamaica)
* en-MY - English (Malaysia)
* en-NZ - English (New Zealand)
* en-PH - English (Philippines)
* en-SG - English (Singapore)
* en-TT - English (Trinidad and Tobago)
* en-US - English (United States)
* en-ZA - English (South Africa)
* en-ZW - English (Zimbabwe)


## Estonian
* et - Estonian
* et-EE - Estonian (Estonia)


## Faroese
* fo - Faroese
* fo-FO - Faroese (Faroe Islands)


## Finnish
* fi - Finnish
* fi-FI - Finnish (Finland)


## French
* fr - French
* fr-BE - French (Belgium)
* fr-CA - French (Canada)
* fr-CD - French Congo (DRC)
* fr-CH - French (Switzerland)
* fr-CI - French (Côte d’Ivoire)
* fr-CM - French (Cameroon)
* fr-FR - French (France)
* fr-HT - French (Haiti)
* fr-LU - French (Luxembourg)
* fr-MA - French (Morocco)
* fr-MC - French (Monaco)
* fr-ML - French (Mali)
* fr-RE - French (Réunion)
* fr-SN - French (Senegal)


## German
* de - German
* de-AT - German (Austria)
* de-CH - German (Switzerland)
* de-DE - German (Germany)
* de-LI - German (Liechtenstein)
* de-LU - German (Luxembourg)


## Greek
* el - Greek
* el-GR - Greek (Greece)


## Hebrew
* he - Hebrew
* he-IL - Hebrew (Israel)


## Hindi
* hi - Hindi
* hi-IN - Hindi (India)


## Hungarian
* hu - Hungarian
* hu-HU - Hungarian (Hungary)


## Icelandic
* is - Icelandic
* is-IS - Icelandic (Iceland)


## Indonesian
* id - Indonesian
* id-ID - Indonesian (Indonesia)


## Italian
* it - Italian
* it-CH - Italian (Switzerland)
* it-IT - Italian (Italy)


## Japanese
* ja - Japanese
* ja-JP - Japanese (Japan)


## Korean
* ko - Korean
* ko-KR - Korean (Korea)


## Latvian
* lv - Latvian
* lv-LV - Latvian (Latvia)


## Lithuanian
* lt - Lithuanian
* lt-LT - Lithuanian (Lithuania)


## Norwegian
* nb - Norwegian Bokmål
* nb-NO - Norwegian Bokmål (Norway)
* nn - Norwegian Nynorsk
* nn-NO - Norwegian Nynorsk (Norway)
* no - Norwegian


## Polish
* pl - Polish
* pl-PL - Polish (Poland)


## Portuguese
* pt - Portuguese
* pt-BR - Portuguese (Brazil)
* pt-PT - Portuguese (Portugal)


## Romanian
* ro - Romanian
* ro-MD - Romanian (Moldova)
* ro-RO - Romanian (Romania)


## Russian
* ru - Russian
* ru-MD - Russian (Moldova)
* ru-RU - Russian (Russia)


## SerboCroatian
* hr - Croatian
* hr-BA - Croatian (Bosnia and Herzegovina)
* hr-HR - Croatian (Croatia)
* sr - Serbian
* sr-Cyrl - Serbian (Cyrillic)
* sr-Cyrl-BA - Serbian (Cyrillic, Bosnia and Herzegovina)
* sr-Cyrl-ME - Serbian (Cyrillic, Montenegro)
* sr-Cyrl-RS - Serbian (Cyrillic, Serbia)
* sr-Latn - Serbian (Latin)
* sr-Latn-BA - Serbian (Latin, Bosnia and Herzegovina)
* sr-Latn-ME - Serbian (Latin, Montenegro)
* sr-Latn-RS - Serbian (Latin, Serbia)


## Slovak
* sk - Slovak
* sk-SK - Slovak (Slovakia)


## Slovenian
* sl - Slovenian
* sl-SI - Slovenian (Slovenia)


## Spanish
* es - Spanish
* es-AR - Spanish (Argentina)
* es-BO - Spanish (Bolivia)
* es-CL - Spanish (Chile)
* es-CO - Spanish (Colombia)
* es-CR - Spanish (Costa Rica)
* es-CU - Spanish (Cuba)
* es-DO - Spanish (Dominican Republic)
* es-EC - Spanish (Ecuador)
* es-ES - Spanish (Spain, International Sort)
* es-GT - Spanish (Guatemala)
* es-HN - Spanish (Honduras)
* es-MX - Spanish (Mexico)
* es-NI - Spanish (Nicaragua)
* es-PA - Spanish (Panama)
* es-PE - Spanish (Peru)
* es-PR - Spanish (Puerto Rico)
* es-PY - Spanish (Paraguay)
* es-SV - Spanish (El Salvador)
* es-US - Spanish (United States)
* es-UY - Spanish (Uruguay)
* es-VE - Spanish (Venezuela)


## Swedish
* sv - Swedish
* sv-FI - Swedish (Finland)
* sv-SE - Swedish (Sweden)


## Thai
* th - Thai
* th-TH - Thai (Thailand)


## Turkish
* tr - Turkish
* tr-TR - Turkish (Türkiye)


## Ukrainian
* uk - Ukrainian
* uk-UA - Ukrainian (Ukraine)


## Vietnamese
* vi - Vietnamese
* vi-VN - Vietnamese (Vietnam)

