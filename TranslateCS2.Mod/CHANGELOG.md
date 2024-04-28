# ChangeLog Version 1.5

* long language names (more than 31 characters) are cut
* filenames are read case-insensitive
* some more handling between built-in locale-ids and read locale-ids


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