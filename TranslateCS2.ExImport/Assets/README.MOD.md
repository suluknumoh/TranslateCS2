# TranslateCS2.Mod

This mod is intended to sideload custom localizations/translations from JSONs.

# JSON-File-Format

* a .json-file
* should contain a single Object
* this Object should have commaseparated Key-Value-Pairs
* the Key should be the Key that is used by Colossal Order
* the Value should be the translation
* example:


{
"TranslateCS2.LocaleNameLocalizedKey": "Nederlands",
"Options.SECTION[General]": "Algemeen"
}


* the example above contains the Key "TranslateCS2.LocaleNameLocalizedKey"
* if the Key-Value-Pair is missing or its Value contains NonBasicLatinCharacters, the english locale-name is gathered from the region code
if the gathered english locale-name contains NonBasicLatinCharacters, the parent english name is used and shown within the drop down to select a flavor
* if the Key-Value-Pair is present and its Value only contains BasicLatinCharacters, the Value is used and shown within the drop down to select a flavor

# JSON-File-Naming
* the following languages are supported with the listed country and/or country-region code
* due to technical limitations, country and/or country-region codes are limited


## Afrikaans
* af - Afrikaans
* af-NA - Afrikaans (Namibia)
* af-ZA - Afrikaans (South Africa)


## Arabic
* ar - Arabic
* ar-001 - Arabic (World)
* ar-AE - Arabic (United Arab Emirates)
* ar-BH - Arabic (Bahrain)
* ar-DJ - Arabic (Djibouti)
* ar-DZ - Arabic (Algeria)
* ar-EG - Arabic (Egypt)
* ar-ER - Arabic (Eritrea)
* ar-IL - Arabic (Israel)
* ar-IQ - Arabic (Iraq)
* ar-JO - Arabic (Jordan)
* ar-KM - Arabic (Comoros)
* ar-KW - Arabic (Kuwait)
* ar-LB - Arabic (Lebanon)
* ar-LY - Arabic (Libya)
* ar-MA - Arabic (Morocco)
* ar-MR - Arabic (Mauritania)
* ar-OM - Arabic (Oman)
* ar-PS - Arabic (Palestinian Authority)
* ar-QA - Arabic (Qatar)
* ar-SA - Arabic (Saudi Arabia)
* ar-SD - Arabic (Sudan)
* ar-SO - Arabic (Somalia)
* ar-SS - Arabic (South Sudan)
* ar-SY - Arabic (Syria)
* ar-TD - Arabic (Chad)
* ar-TN - Arabic (Tunisia)
* ar-YE - Arabic (Yemen)


## Basque
* eu - Basque
* eu-ES - Basque (Spain)


## Belarusian
* be - Belarusian
* be-BY - Belarusian (Belarus)


## Bulgarian
* bg - Bulgarian
* bg-BG - Bulgarian (Bulgaria)


## Catalan
* ca - Catalan
* ca-AD - Catalan (Andorra)
* ca-ES - Catalan (Spain)
* ca-FR - Catalan (France)
* ca-IT - Catalan (Italy)


## ChineseSimplified
* zh-Hans - Chinese (Simplified)
* zh-Hans-CN - Chinese (Simplified, China)
* zh-Hans-HK - Chinese (Simplified, Hong Kong SAR)
* zh-Hans-MO - Chinese (Simplified, Macao SAR)
* zh-Hans-SG - Chinese (Simplified, Singapore)


## ChineseTraditional
* zh-Hant - Chinese (Traditional)
* zh-Hant-HK - Chinese (Traditional, Hong Kong SAR)
* zh-Hant-MO - Chinese (Traditional, Macao SAR)
* zh-Hant-TW - Chinese (Traditional, Taiwan)


## Czech
* cs - Czech
* cs-CZ - Czech (Czechia)


## Danish
* da - Danish
* da-DK - Danish (Denmark)
* da-GL - Danish (Greenland)


## Dutch
* nl - Dutch
* nl-AW - Dutch (Aruba)
* nl-BE - Dutch (Belgium)
* nl-BQ - Dutch (Bonaire, Sint Eustatius and Saba)
* nl-CW - Dutch
* nl-NL - Dutch (Netherlands)
* nl-SR - Dutch (Suriname)
* nl-SX - Dutch (Sint Maarten)


## English
* en - English
* en-001 - English (World)
* en-029 - English (Caribbean)
* en-150 - English (Europe)
* en-AE - English (United Arab Emirates)
* en-AG - English (Antigua and Barbuda)
* en-AI - English (Anguilla)
* en-AS - English (American Samoa)
* en-AT - English (Austria)
* en-AU - English (Australia)
* en-BB - English (Barbados)
* en-BE - English (Belgium)
* en-BI - English (Burundi)
* en-BM - English (Bermuda)
* en-BS - English (Bahamas)
* en-BW - English (Botswana)
* en-BZ - English (Belize)
* en-CA - English (Canada)
* en-CC - English (Cocos [Keeling] Islands)
* en-CH - English (Switzerland)
* en-CK - English (Cook Islands)
* en-CM - English (Cameroon)
* en-CX - English (Christmas Island)
* en-CY - English (Cyprus)
* en-DE - English (Germany)
* en-DK - English (Denmark)
* en-DM - English (Dominica)
* en-ER - English (Eritrea)
* en-FI - English (Finland)
* en-FJ - English (Fiji)
* en-FK - English (Falkland Islands)
* en-FM - English (Micronesia)
* en-GB - English (United Kingdom)
* en-GD - English (Grenada)
* en-GG - English (Guernsey)
* en-GH - English (Ghana)
* en-GI - English (Gibraltar)
* en-GM - English (Gambia)
* en-GU - English (Guam)
* en-GY - English (Guyana)
* en-HK - English (Hong Kong SAR)
* en-ID - English (Indonesia)
* en-IE - English (Ireland)
* en-IL - English (Israel)
* en-IM - English (Isle of Man)
* en-IN - English (India)
* en-IO - English (British Indian Ocean Territory)
* en-JE - English (Jersey)
* en-JM - English (Jamaica)
* en-KE - English (Kenya)
* en-KI - English (Kiribati)
* en-KN - English (St. Kitts and Nevis)
* en-KY - English (Cayman Islands)
* en-LC - English (St. Lucia)
* en-LR - English (Liberia)
* en-LS - English (Lesotho)
* en-MG - English (Madagascar)
* en-MH - English (Marshall Islands)
* en-MO - English (Macao SAR)
* en-MP - English (Northern Mariana Islands)
* en-MS - English (Montserrat)
* en-MT - English (Malta)
* en-MU - English (Mauritius)
* en-MW - English (Malawi)
* en-MY - English (Malaysia)
* en-NA - English (Namibia)
* en-NF - English (Norfolk Island)
* en-NG - English (Nigeria)
* en-NL - English (Netherlands)
* en-NR - English (Nauru)
* en-NU - English (Niue)
* en-NZ - English (New Zealand)
* en-PG - English (Papua New Guinea)
* en-PH - English (Philippines)
* en-PK - English (Pakistan)
* en-PN - English (Pitcairn Islands)
* en-PR - English (Puerto Rico)
* en-PW - English (Palau)
* en-RW - English (Rwanda)
* en-SB - English (Solomon Islands)
* en-SC - English (Seychelles)
* en-SD - English (Sudan)
* en-SE - English (Sweden)
* en-SG - English (Singapore)
* en-SH - English (St Helena, Ascension, Tristan da Cunha)
* en-SI - English (Slovenia)
* en-SL - English (Sierra Leone)
* en-SS - English (South Sudan)
* en-SX - English (Sint Maarten)
* en-SZ - English (Eswatini)
* en-TC - English (Turks and Caicos Islands)
* en-TK - English (Tokelau)
* en-TO - English (Tonga)
* en-TT - English (Trinidad and Tobago)
* en-TV - English (Tuvalu)
* en-TZ - English (Tanzania)
* en-UG - English (Uganda)
* en-UM - English (U.S. Outlying Islands)
* en-US - English (United States)
* en-US-POSIX - English (United States, Computer)
* en-VC - English (St. Vincent and Grenadines)
* en-VG - English (British Virgin Islands)
* en-VI - English (U.S. Virgin Islands)
* en-VU - English (Vanuatu)
* en-WS - English (Samoa)
* en-ZA - English (South Africa)
* en-ZM - English (Zambia)
* en-ZW - English (Zimbabwe)


## Estonian
* et - Estonian
* et-EE - Estonian (Estonia)


## Faroese
* fo - Faroese
* fo-DK - Faroese (Denmark)
* fo-FO - Faroese (Faroe Islands)


## Finnish
* fi - Finnish
* fi-FI - Finnish (Finland)


## French
* fr - French
* fr-029 - French (Caribbean)
* fr-BE - French (Belgium)
* fr-BF - French (Burkina Faso)
* fr-BI - French (Burundi)
* fr-BJ - French (Benin)
* fr-BL - French
* fr-CA - French (Canada)
* fr-CD - French (Congo [DRC])
* fr-CF - French (Central African Republic)
* fr-CG - French (Congo)
* fr-CH - French (Switzerland)
* fr-CI - French
* fr-CM - French (Cameroon)
* fr-DJ - French (Djibouti)
* fr-DZ - French (Algeria)
* fr-FR - French (France)
* fr-GA - French (Gabon)
* fr-GF - French (French Guiana)
* fr-GN - French (Guinea)
* fr-GP - French (Guadeloupe)
* fr-GQ - French (Equatorial Guinea)
* fr-HT - French (Haiti)
* fr-KM - French (Comoros)
* fr-LU - French (Luxembourg)
* fr-MA - French (Morocco)
* fr-MC - French (Monaco)
* fr-MF - French (St. Martin)
* fr-MG - French (Madagascar)
* fr-ML - French (Mali)
* fr-MQ - French (Martinique)
* fr-MR - French (Mauritania)
* fr-MU - French (Mauritius)
* fr-NC - French (New Caledonia)
* fr-NE - French (Niger)
* fr-PF - French (French Polynesia)
* fr-PM - French (St. Pierre and Miquelon)
* fr-RE - French
* fr-RW - French (Rwanda)
* fr-SC - French (Seychelles)
* fr-SN - French (Senegal)
* fr-SY - French (Syria)
* fr-TD - French (Chad)
* fr-TG - French (Togo)
* fr-TN - French (Tunisia)
* fr-VU - French (Vanuatu)
* fr-WF - French (Wallis and Futuna)
* fr-YT - French (Mayotte)


## German
* de - German
* de-AT - German (Austria)
* de-BE - German (Belgium)
* de-CH - German (Switzerland)
* de-DE - German (Germany)
* de-IT - German (Italy)
* de-LI - German (Liechtenstein)
* de-LU - German (Luxembourg)


## Greek
* el - Greek
* el-CY - Greek (Cyprus)
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
* it-SM - Italian (San Marino)
* it-VA - Italian (Vatican City)


## Japanese
* ja - Japanese
* ja-JP - Japanese (Japan)


## Korean
* ko - Korean
* ko-KP - Korean (North Korea)
* ko-KR - Korean (Korea)


## Latvian
* lv - Latvian
* lv-LV - Latvian (Latvia)


## Lithuanian
* lt - Lithuanian
* lt-LT - Lithuanian (Lithuania)


## Norwegian
* nb - Invariant Language (Invariant Country)
* nb-NO - Invariant Language (Invariant Country)
* nb-SJ - Invariant Language (Invariant Country)
* nn - Norwegian Nynorsk
* nn-NO - Norwegian Nynorsk (Norway)


## Polish
* pl - Polish
* pl-PL - Polish (Poland)


## Portuguese
* pt - Portuguese
* pt-AO - Portuguese (Angola)
* pt-BR - Portuguese (Brazil)
* pt-CH - Portuguese (Switzerland)
* pt-CV - Portuguese (Cabo Verde)
* pt-GQ - Portuguese (Equatorial Guinea)
* pt-GW - Portuguese (Guinea-Bissau)
* pt-LU - Portuguese (Luxembourg)
* pt-MO - Portuguese (Macao SAR)
* pt-MZ - Portuguese (Mozambique)
* pt-PT - Portuguese (Portugal)
* pt-ST - Portuguese
* pt-TL - Portuguese (Timor-Leste)


## Romanian
* ro - Romanian
* ro-MD - Romanian (Moldova)
* ro-RO - Romanian (Romania)


## Russian
* ru - Russian
* ru-BY - Russian (Belarus)
* ru-KG - Russian (Kyrgyzstan)
* ru-KZ - Russian (Kazakhstan)
* ru-MD - Russian (Moldova)
* ru-RU - Russian (Russia)
* ru-UA - Russian (Ukraine)


## SerboCroatian
* hr - Croatian
* hr-BA - Croatian (Bosnia and Herzegovina)
* hr-HR - Croatian (Croatia)
* sr - Serbian
* sr-Cyrl - Serbian (Cyrillic)
* sr-Cyrl-BA - Serbian (Cyrillic, Bosnia and Herzegovina)
* sr-Cyrl-ME - Serbian (Cyrillic, Montenegro)
* sr-Cyrl-RS - Serbian (Cyrillic, Serbia)
* sr-Cyrl-XK - Serbian (Cyrillic, Kosovo)
* sr-Latn - Serbian (Latin)
* sr-Latn-BA - Serbian (Latin, Bosnia and Herzegovina)
* sr-Latn-ME - Serbian (Latin, Montenegro)
* sr-Latn-RS - Serbian (Latin, Serbia)
* sr-Latn-XK - Serbian (Latin, Kosovo)


## Slovak
* sk - Slovak
* sk-SK - Slovak (Slovakia)


## Slovenian
* sl - Slovenian
* sl-SI - Slovenian (Slovenia)


## Spanish
* es - Spanish
* es-419 - Spanish (Latin America)
* es-AR - Spanish (Argentina)
* es-BO - Spanish (Bolivia)
* es-BR - Spanish (Brazil)
* es-BZ - Spanish (Belize)
* es-CL - Spanish (Chile)
* es-CO - Spanish (Colombia)
* es-CR - Spanish (Costa Rica)
* es-CU - Spanish (Cuba)
* es-DO - Spanish (Dominican Republic)
* es-EC - Spanish (Ecuador)
* es-ES - Spanish (Spain)
* es-GQ - Spanish (Equatorial Guinea)
* es-GT - Spanish (Guatemala)
* es-HN - Spanish (Honduras)
* es-MX - Spanish (Mexico)
* es-NI - Spanish (Nicaragua)
* es-PA - Spanish (Panama)
* es-PE - Spanish (Peru)
* es-PH - Spanish (Philippines)
* es-PR - Spanish (Puerto Rico)
* es-PY - Spanish (Paraguay)
* es-SV - Spanish (El Salvador)
* es-US - Spanish (United States)
* es-UY - Spanish (Uruguay)
* es-VE - Spanish (Venezuela)


## Swedish
* sv - Swedish
* sv-AX - Swedish
* sv-FI - Swedish (Finland)
* sv-SE - Swedish (Sweden)


## Thai
* th - Thai
* th-TH - Thai (Thailand)


## Turkish
* tr - Turkish
* tr-CY - Turkish (Cyprus)
* tr-TR - Turkish (Turkey)


## Ukrainian
* uk - Ukrainian
* uk-UA - Ukrainian (Ukraine)


## Vietnamese
* vi - Vietnamese
* vi-VN - Vietnamese (Vietnam)



# JSON-File-Location

* the .json-file should be placed into the following directory:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\
# ChangeLog Version 1.1

## File-System/Locations

Translations/Localizations now have to be placed into the following directory:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\


Settings can now be found over there:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsSettings\TranslateCS2.Mod\


## Mod-Settings within the game

* removed behaviour -> add as source
* removed clear overwritten
* added drop down to select a flavor
* if a certain language has no flavors, the drop down is disabled
* if a flavor for a built in language is added, the drop down has an entry 'none'
this entry replaces the removed 'clear overwritten'
* if a 'new' language is added, the drop down misses the entry 'none'