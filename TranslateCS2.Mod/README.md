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
* af         - Afrikaans                                          - Afrikaans
* af-ZA      - Afrikaans (South Africa)                           - Afrikaans (Suid-Afrika)


## Arabic
* ar         - Arabic                                             - العربية
* ar-AE      - Arabic (United Arab Emirates)                      - العربية (الإمارات العربية المتحدة)
* ar-BH      - Arabic (Bahrain)                                   - العربية (البحرين)
* ar-DZ      - Arabic (Algeria)                                   - العربية (الجزائر)
* ar-EG      - Arabic (Egypt)                                     - العربية (مصر)
* ar-IQ      - Arabic (Iraq)                                      - العربية (العراق)
* ar-JO      - Arabic (Jordan)                                    - العربية (الأردن)
* ar-KW      - Arabic (Kuwait)                                    - العربية (الكويت)
* ar-LB      - Arabic (Lebanon)                                   - العربية (لبنان)
* ar-LY      - Arabic (Libya)                                     - العربية (ليبيا)
* ar-MA      - Arabic (Morocco)                                   - العربية (المغرب)
* ar-OM      - Arabic (Oman)                                      - العربية (عُمان)
* ar-QA      - Arabic (Qatar)                                     - العربية (قطر)
* ar-SA      - Arabic (Saudi Arabia)                              - العربية (المملكة العربية السعودية)
* ar-SY      - Arabic (Syria)                                     - العربية (سوريا)
* ar-TN      - Arabic (Tunisia)                                   - العربية (تونس)
* ar-YE      - Arabic (Yemen)                                     - العربية (اليمن)


## Basque
* eu         - Basque                                             - euskara
* eu-ES      - Basque (Spain)                                     - euskara (Espainia)


## Belarusian
* be         - Belarusian                                         - беларуская
* be-BY      - Belarusian (Belarus)                               - беларуская (Беларусь)


## Bulgarian
* bg         - Bulgarian                                          - български
* bg-BG      - Bulgarian (Bulgaria)                               - български (България)


## Catalan
* ca         - Catalan                                            - català
* ca-ES      - Catalan (Spain)                                    - català (Espanya)
* ca-ES-valencia - Catalan (Spain)                                    - català (Espanya)


## ChineseSimplified
* zh         - Chinese (Simplified)                               - 中文
* zh-CHS     - Chinese (Simplified) Legacy                        - 中文
* zh-CN      - Chinese (Simplified)                               - 中文 (中国)
* zh-Hans    - Chinese (Simplified)                               - 中文
* zh-SG      - Chinese (Simplified, Singapore)                    - 中文 (新加坡)


## ChineseTraditional
* zh-CHT     - Chinese (Traditional) Legacy                       - 中文
* zh-Hant    - Chinese (Traditional)                              - 中文
* zh-HK      - Chinese (Traditional, Hong Kong SAR China)         - 中文 (中国香港特别行政区)
* zh-MO      - Chinese (Traditional, Macau SAR China)             - 中文 (中国澳门特别行政区)
* zh-TW      - Chinese (Traditional)                              - 中文 (台湾)


## Czech
* cs         - Czech                                              - čeština
* cs-CZ      - Czech (Czech Republic)                             - čeština (Česká republika)


## Danish
* da         - Danish                                             - dansk
* da-DK      - Danish (Denmark)                                   - dansk (Danmark)


## Dutch
* nl         - Dutch                                              - Nederlands
* nl-BE      - Dutch (Belgium)                                    - Nederlands (België)
* nl-NL      - Dutch (Netherlands)                                - Nederlands (Nederland)


## English
* en         - English                                            - English
* en-AU      - English (Australia)                                - English (Australia)
* en-BZ      - English (Belize)                                   - English (Belize)
* en-CA      - English (Canada)                                   - English (Canada)
* en-GB      - English (United Kingdom)                           - English (United Kingdom)
* en-HK      - English (Hong Kong SAR China)                      - English (Hong Kong SAR China)
* en-IE      - English (Ireland)                                  - English (Ireland)
* en-IN      - English (India)                                    - English (India)
* en-JM      - English (Jamaica)                                  - English (Jamaica)
* en-MY      - English (Malaysia)                                 - English (Malaysia)
* en-NZ      - English (New Zealand)                              - English (New Zealand)
* en-PH      - English (Philippines)                              - English (Philippines)
* en-SG      - English (Singapore)                                - English (Singapore)
* en-TT      - English (Trinidad and Tobago)                        - English (Trinidad and Tobago)
* en-US      - English (United States)                            - English (United States)
* en-ZA      - English (South Africa)                             - English (South Africa)
* en-ZW      - English (Zimbabwe)                                 - English (Zimbabwe)


## Estonian
* et         - Estonian                                           - eesti
* et-EE      - Estonian (Estonia)                                 - eesti (Eesti)


## Faroese
* fo         - Faroese                                            - føroyskt
* fo-FO      - Faroese (Faroe Islands)                            - føroyskt (Føroyar)


## Finnish
* fi         - Finnish                                            - suomi
* fi-FI      - Finnish (Finland)                                  - suomi (Suomi)


## French
* fr         - French                                             - français
* fr-BE      - French (Belgium)                                   - français (Belgique)
* fr-CA      - French (Canada)                                    - français (Canada)
* fr-CD      - French (Congo - Kinshasa)                          - français (Congo-Kinshasa)
* fr-CH      - French (Switzerland)                               - français (Suisse)
* fr-CI      - French (Côte d’Ivoire)                             - français (Côte d’Ivoire)
* fr-CM      - French (Cameroon)                                  - français (Cameroun)
* fr-FR      - French (France)                                    - français (France)
* fr-HT      - French (Haiti)                                     - français (Haïti)
* fr-LU      - French (Luxembourg)                                - français (Luxembourg)
* fr-MA      - French (Morocco)                                   - français (Maroc)
* fr-MC      - French (Monaco)                                    - français (Monaco)
* fr-ML      - French (Mali)                                      - français (Mali)
* fr-RE      - French (Réunion)                                   - français (La Réunion)
* fr-SN      - French (Senegal)                                   - français (Sénégal)


## German
* de         - German                                             - Deutsch
* de-AT      - German (Austria)                                   - Deutsch (Österreich)
* de-CH      - German (Switzerland)                               - Deutsch (Schweiz)
* de-DE      - German (Germany)                                   - Deutsch (Deutschland)
* de-LI      - German (Liechtenstein)                             - Deutsch (Liechtenstein)
* de-LU      - German (Luxembourg)                                - Deutsch (Luxemburg)


## Greek
* el         - Greek                                              - Ελληνικά
* el-GR      - Greek (Greece)                                     - Ελληνικά (Ελλάδα)


## Hebrew
* he         - Hebrew                                             - עברית
* he-IL      - Hebrew (Israel)                                    - עברית (ישראל)


## Hindi
* hi         - Hindi                                              - हिन्दी
* hi-IN      - Hindi (India)                                      - हिन्दी (भारत)


## Hungarian
* hu         - Hungarian                                          - magyar
* hu-HU      - Hungarian (Hungary)                                - magyar (Magyarország)


## Icelandic
* is         - Icelandic                                          - íslenska
* is-IS      - Icelandic (Iceland)                                - íslenska (Ísland)


## Indonesian
* id         - Indonesian                                         - Indonesia
* id-ID      - Indonesian (Indonesia)                             - Indonesia (Indonesia)


## Italian
* it         - Italian                                            - italiano
* it-CH      - Italian (Switzerland)                              - italiano (Svizzera)
* it-IT      - Italian (Italy)                                    - italiano (Italia)


## Japanese
* ja         - Japanese                                           - 日本語
* ja-JP      - Japanese (Japan)                                   - 日本語 (日本)


## Korean
* ko         - Korean                                             - 한국어
* ko-KR      - Korean (South Korea)                               - 한국어 (대한민국)


## Latvian
* lv         - Latvian                                            - latviešu
* lv-LV      - Latvian (Latvia)                                   - latviešu (Latvija)


## Lithuanian
* lt         - Lithuanian                                         - lietuvių
* lt-LT      - Lithuanian (Lithuania)                             - lietuvių (Lietuva)


## Norwegian
* nb         - Norwegian Bokmål                                   - norsk bokmål
* nb-NO      - Norwegian Bokmål (Norway)                          - norsk bokmål (Norge)
* nn         - Norwegian Nynorsk                                  - nynorsk
* nn-NO      - Norwegian Nynorsk (Norway)                         - nynorsk (Noreg)
* no         - Norwegian                                          - norsk


## Polish
* pl         - Polish                                             - polski
* pl-PL      - Polish (Poland)                                    - polski (Polska)


## Portuguese
* pt         - Portuguese                                         - português
* pt-BR      - Portuguese (Brazil)                                - português (Brasil)
* pt-PT      - Portuguese (Portugal)                              - português (Portugal)


## Romanian
* ro         - Romanian                                           - română
* ro-MD      - Romanian (Moldova)                                 - română (Republica Moldova)
* ro-RO      - Romanian (Romania)                                 - română (România)


## Russian
* ru         - Russian                                            - русский
* ru-MD      - Russian (Moldova)                                  - русский (Молдова)
* ru-RU      - Russian (Russia)                                   - русский (Россия)


## SerboCroatian
* hr         - Croatian                                           - hrvatski
* hr-BA      - Croatian (Bosnia and Herzegovina)                    - hrvatski (Bosna i Hercegovina)
* hr-HR      - Croatian (Croatia)                                 - hrvatski (Hrvatska)
* sr         - Serbian                                            - српски
* sr-Cyrl    - Serbian (Cyrillic)                                 - српски
* sr-Cyrl-BA - Serbian (Cyrillic, Bosnia and Herzegovina)           - српски (Босна и Херцеговина)
* sr-Cyrl-ME - Serbian (Cyrillic, Montenegro)                     - српски (Црна Гора)
* sr-Cyrl-RS - Serbian (Cyrillic, Serbia)                         - српски (Србија)
* sr-Latn    - Serbian (Latin)                                    - српски
* sr-Latn-BA - Serbian (Latin, Bosnia and Herzegovina)              - српски (Босна и Херцеговина)
* sr-Latn-ME - Serbian (Latin, Montenegro)                        - српски (Црна Гора)
* sr-Latn-RS - Serbian (Latin, Serbia)                            - српски (Србија)


## Slovak
* sk         - Slovak                                             - slovenčina
* sk-SK      - Slovak (Slovakia)                                  - slovenčina (Slovensko)


## Slovenian
* sl         - Slovenian                                          - slovenščina
* sl-SI      - Slovenian (Slovenia)                               - slovenščina (Slovenija)


## Spanish
* es         - Spanish                                            - español
* es-AR      - Spanish (Argentina)                                - español (Argentina)
* es-BO      - Spanish (Bolivia)                                  - español (Bolivia)
* es-CL      - Spanish (Chile)                                    - español (Chile)
* es-CO      - Spanish (Colombia)                                 - español (Colombia)
* es-CR      - Spanish (Costa Rica)                               - español (Costa Rica)
* es-CU      - Spanish (Cuba)                                     - español (Cuba)
* es-DO      - Spanish (Dominican Republic)                       - español (República Dominicana)
* es-EC      - Spanish (Ecuador)                                  - español (Ecuador)
* es-ES      - Spanish (Spain)                                    - español (España)
* es-GT      - Spanish (Guatemala)                                - español (Guatemala)
* es-HN      - Spanish (Honduras)                                 - español (Honduras)
* es-MX      - Spanish (Mexico)                                   - español (México)
* es-NI      - Spanish (Nicaragua)                                - español (Nicaragua)
* es-PA      - Spanish (Panama)                                   - español (Panamá)
* es-PE      - Spanish (Peru)                                     - español (Perú)
* es-PR      - Spanish (Puerto Rico)                              - español (Puerto Rico)
* es-PY      - Spanish (Paraguay)                                 - español (Paraguay)
* es-SV      - Spanish (El Salvador)                              - español (El Salvador)
* es-US      - Spanish (United States)                            - español (Estados Unidos)
* es-UY      - Spanish (Uruguay)                                  - español (Uruguay)
* es-VE      - Spanish (Venezuela)                                - español (Venezuela)


## Swedish
* sv         - Swedish                                            - svenska
* sv-FI      - Swedish (Finland)                                  - svenska (Finland)
* sv-SE      - Swedish (Sweden)                                   - svenska (Sverige)


## Thai
* th         - Thai                                               - ไทย
* th-TH      - Thai (Thailand)                                    - ไทย (ไทย)


## Turkish
* tr         - Turkish                                            - Türkçe
* tr-TR      - Turkish (Turkey)                                   - Türkçe (Türkiye)


## Ukrainian
* uk         - Ukrainian                                          - українська
* uk-UA      - Ukrainian (Ukraine)                                - українська (Україна)


## Vietnamese
* vi         - Vietnamese                                         - Tiếng Việt
* vi-VN      - Vietnamese (Vietnam)                               - Tiếng Việt (Việt Nam)

