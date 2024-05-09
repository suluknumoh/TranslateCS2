# Version 2.0
- display languages english name within this mods settings for labels and descriptions
- button to reload languages is only visible for developers
- developers have the ability to generate a .json-file with this mods Keys and Values/Texts/Translations
- updated supported languages
- added supported languages:
    - no - Norwegian
    - ChineseSimplified
        - zh-CN - Chinese (Simplified, China)
        - zh-SG - Chinese (Simplified, Singapore)
        - zh-CHS - Chinese (Simplified) Legacy
    - ChineseTraditional
        - zh-HK - Chinese (Traditional, Hong Kong SAR)
        - zh-MO - Chinese (Traditional, Macao SAR)
        - zh-TW - Chinese (Traditional, Taiwan)
        - zh-CHT - Chinese (Traditional) Legacy
# ChangeLog Version 1.5.2

* added appendix '(Latin)' or '(Cyrillic)' for Serbian languages within the flavor-drop-down to be able to differentiate them
* the language drop down within the interface settings now lists 'српски/hrvatski' instead of 'SerboCroatian'
* if provided json-files are corrupt on load, an error message is shown (an example screenshot is available within the gallery and within this mods examples directory)
* if provided json-files are corrupt on reload, an error message is shown (an example screenshot is available within the gallery and within this mods examples directory)
* performance: just as an info and at least on my machine: it takes amongst 3.5 seconds to load all 167 (non-corrupt) language-/translation-files with a total of 2,942,874 entries (17,622 entries per language-/translation-file)
* added Disclaimer to Readme/LongDescription
* tried to explain it a bit more


# ChangeLog Version 1.5.1

* native names are displayed instead of english names
* reduced examples to mappable examples
* reduced LocalesSupported to mappable language-region-codes


# ChangeLog Version 1.5

* long language names (more than 31 characters) are cut
* filenames are read case-insensitive
* some more handling between built-in locale-ids and read locale-ids
* update github link to mods readme.md


# ChangeLog Version 1.4

* fix missing pre-init for non built-in languages
* fix upper-lower-cases (for example: zh-Hans versus zh-HANS)
* example jsons for each supported language-region-code added
* images and examples wont be embedded anymore to reduce the dlls size


# ChangeLog Version 1.3

## fix on load error
* error on loading the mod and prevented saved flavor selections from beeing restored


# ChangeLog Version 1.2

## display name
* correct stepping to display at least the correct english name

## reload language
* add to flavormapping on load, otherwise language source got removed, but not added


# ChangeLog Version 1.1

## File-System/Locations

Translations/Localizations now have to be placed into the following directory:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\


Settings can now be found over there:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsSettings\TranslateCS2.Mod\


## Mod-Settings within the game

* removed behaviour -&gt; add as source
* removed clear overwritten
* added drop down to select a flavor
* if a certain language has no flavors, the drop down is disabled
* if a flavor for a built in language is added, the drop down has an entry 'none'; this entry replaces the removed 'clear overwritten'
* if a 'new' language is added, the drop down misses the entry 'none'