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
* af-ZA - Afrikaans (Suid-Afrika)


## Arabic
* ar - Arabic
* ar-SA - Arabic (Saudi Arabia)
* ar-IQ - Arabic (Iraq)
* ar-EG - Arabic (Egypt)
* ar-LY - Arabic (Libya)
* ar-DZ - Arabic (Algeria)
* ar-MA - Arabic (Morocco)
* ar-TN - Arabic (Tunisia)
* ar-OM - Arabic (Oman)
* ar-YE - Arabic (Yemen)
* ar-SY - Arabic (Syria)
* ar-JO - Arabic (Jordan)
* ar-LB - Arabic (Lebanon)
* ar-KW - Arabic (Kuwait)
* ar-AE - Arabic (United Arab Emirates)
* ar-BH - Arabic (Bahrain)
* ar-QA - Arabic (Qatar)


## Basque
* eu - euskara
* eu-ES - euskara (Espainia)


## Belarusian
* be - Belarusian
* be-BY - Belarusian (Belarus)


## Bulgarian
* bg - Bulgarian
* bg-BG - Bulgarian (Bulgaria)


## Catalan
* ca - Catalan
* ca-ES - Catalan (Spain)
* ca-ES-valencia - Catalan (Spain)


## ChineseSimplified
* zh-Hans - Chinese (Simplified)
* zh-CHS - Chinese (Simplified) Legacy
* zh-CN - Chinese (Simplified)
* zh-SG - Chinese (Simplified, Singapore)
* zh - Chinese (Simplified)


## ChineseTraditional
* zh-TW - Chinese (Traditional)
* zh-HK - Chinese (Traditional, Hong Kong SAR China)
* zh-MO - Chinese (Traditional, Macau SAR China)
* zh-Hant - Chinese (Traditional)
* zh-CHT - Chinese (Traditional) Legacy


## Czech
* cs - Czech
* cs-CZ - Czech (Czech Republic)


## Danish
* da - dansk
* da-DK - dansk (Danmark)


## Dutch
* nl - Nederlands
* nl-NL - Nederlands (Nederland)
* nl-BE - Dutch (Belgium)


## English
* en - English
* en-US - English (United States)
* en-GB - English (United Kingdom)
* en-AU - English (Australia)
* en-CA - English (Canada)
* en-NZ - English (New Zealand)
* en-IE - English (Ireland)
* en-ZA - English (South Africa)
* en-JM - English (Jamaica)
* en-BZ - English (Belize)
* en-TT - English (Trinidad and Tobago)
* en-ZW - English (Zimbabwe)
* en-PH - English (Philippines)
* en-HK - English (Hong Kong SAR China)
* en-IN - English (India)
* en-MY - English (Malaysia)
* en-SG - English (Singapore)


## Estonian
* et - eesti
* et-EE - eesti (Eesti)


## Faroese
* fo - Faroese
* fo-FO - Faroese (Faroe Islands)


## Finnish
* fi - suomi
* fi-FI - suomi (Suomi)


## French
* fr - French
* fr-FR - French (France)
* fr-BE - French (Belgium)
* fr-CA - French (Canada)
* fr-CH - French (Switzerland)
* fr-LU - French (Luxembourg)
* fr-MC - French (Monaco)
* fr-RE - French
* fr-CD - French (Congo - Kinshasa)
* fr-SN - French (Senegal)
* fr-CM - French (Cameroon)
* fr-CI - French
* fr-ML - French (Mali)
* fr-MA - French (Morocco)
* fr-HT - French (Haiti)


## German
* de - Deutsch
* de-DE - Deutsch (Deutschland)
* de-CH - Deutsch (Schweiz)
* de-AT - German (Austria)
* de-LU - Deutsch (Luxemburg)
* de-LI - Deutsch (Liechtenstein)


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
* hu - magyar
* hu-HU - Hungarian (Hungary)


## Icelandic
* is - Icelandic
* is-IS - Icelandic (Iceland)


## Indonesian
* id - Indonesia
* id-ID - Indonesia (Indonesia)


## Italian
* it - italiano
* it-IT - italiano (Italia)
* it-CH - italiano (Svizzera)


## Japanese
* ja - Japanese
* ja-JP - Japanese (Japan)


## Korean
* ko - Korean
* ko-KR - Korean (South Korea)


## Latvian
* lv - Latvian
* lv-LV - Latvian (Latvia)


## Lithuanian
* lt - Lithuanian
* lt-LT - Lithuanian (Lithuania)


## Norwegian
* no - norsk
* nb-NO - nb
* nn-NO - nynorsk (Noreg)
* nn - nynorsk
* nb - Norwegian


## Polish
* pl - polski
* pl-PL - polski (Polska)


## Portuguese
* pt - Portuguese
* pt-BR - Portuguese (Brazil)
* pt-PT - Portuguese (Portugal)


## Romanian
* ro - Romanian
* ro-RO - Romanian (Romania)
* ro-MD - Romanian (Moldova)


## Russian
* ru - Russian
* ru-RU - Russian (Russia)
* ru-MD - Russian (Moldova)


## SerboCroatian
* hr - hrvatski
* hr-HR - hrvatski (Hrvatska)
* hr-BA - hrvatski (Bosna i Hercegovina)
* sr-Latn-BA - Serbian (Latin, Bosnia and Herzegovina)
* sr-Cyrl-BA - Serbian (Cyrillic, Bosnia and Herzegovina)
* sr-Latn-RS - Serbian (Latin, Serbia)
* sr-Cyrl-RS - Serbian (Cyrillic, Serbia)
* sr-Latn-ME - Serbian (Latin, Montenegro)
* sr-Cyrl-ME - Serbian (Cyrillic, Montenegro)
* sr-Cyrl - Serbian (Cyrillic)
* sr-Latn - Serbian (Latin)
* sr - Serbian


## Slovak
* sk - Slovak
* sk-SK - Slovak (Slovakia)


## Slovenian
* sl - Slovenian
* sl-SI - Slovenian (Slovenia)


## Spanish
* es - Spanish
* es-MX - Spanish (Mexico)
* es-ES - Spanish (Spain)
* es-GT - Spanish (Guatemala)
* es-CR - Spanish (Costa Rica)
* es-PA - Spanish (Panama)
* es-DO - Spanish (Dominican Republic)
* es-VE - Spanish (Venezuela)
* es-CO - Spanish (Colombia)
* es-PE - Spanish (Peru)
* es-AR - Spanish (Argentina)
* es-EC - Spanish (Ecuador)
* es-CL - Spanish (Chile)
* es-UY - Spanish (Uruguay)
* es-PY - Spanish (Paraguay)
* es-BO - Spanish (Bolivia)
* es-SV - Spanish (El Salvador)
* es-HN - Spanish (Honduras)
* es-NI - Spanish (Nicaragua)
* es-PR - Spanish (Puerto Rico)
* es-US - Spanish (United States)
* es-CU - Spanish (Cuba)


## Swedish
* sv - svenska
* sv-SE - svenska (Sverige)
* sv-FI - svenska (Finland)


## Thai
* th - Thai
* th-TH - Thai (Thailand)


## Turkish
* tr - Turkish
* tr-TR - Turkish (Turkey)


## Ukrainian
* uk - Ukrainian
* uk-UA - Ukrainian (Ukraine)


## Vietnamese
* vi - Vietnamese
* vi-VN - Vietnamese (Vietnam)

