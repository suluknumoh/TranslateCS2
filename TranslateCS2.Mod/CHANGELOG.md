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