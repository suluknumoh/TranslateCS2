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
* full list can be found over there: https://github.com/suluknumoh/TranslateCS2/blob/main/TranslateCS2.Mod/LANGUAGES.SUPPORTED.md

# Support for this Mod
you are welcome to leave concerns, issues, feedback and/or suggestions on the following platforms

## Cities: Skylines Modding Discord
* feel free to join [Cities: Skylines Modding](https://discord.gg/HTav7ARPs2)-Discord
* this mod has a channel within it (https://discord.com/channels/1024242828114673724/1256524100462055445)

## GitHub
* [TranslateCS2 - Issues](https://github.com/suluknumoh/TranslateCS2/issues)

# Credits

## i286-1
- https://github.com/i286-1
- for:
    - testing and giving feedback
        - https://github.com/suluknumoh/TranslateCS2/issues/17
