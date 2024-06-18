# Disclaimer
This is an unofficial tool provided as is to use at your own risk.

Please see also: [MIT-License](https://github.com/suluknumoh/TranslateCS2?tab=MIT-1-ov-file)

# Intention
- this thread [Convert .loc file to .csv and vice versa](https://forum.paradoxplaza.com/forum/threads/convert-loc-file-to-csv-and-vice-versa.1627477/)
- especially [ElroyNL](https://forum.paradoxplaza.com/forum/members/elroynl.1385312/)s [post](https://forum.paradoxplaza.com/forum/threads/convert-loc-file-to-csv-and-vice-versa.1627477/post-29451494)

# Requirements
## general
[Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
via
[STEAM](https://store.steampowered.com)
on
[Microsoft Windows](https://www.microsoft.com/windows/)


This tool can not be used without having [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installed via [STEAM](https://store.steampowered.com) on [Microsoft Windows](https://www.microsoft.com/windows/)

## Self-Containing release
No further requirements

## Dependent release
[.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

# Limitations
## Microsoft Versions (Store, GamePass)
The game files seem to be encrypted, protected and/or hidden, so this application cannot access them.

- https://www.reddit.com/r/pcgaming/comments/c39dgs/does_anyone_know_where_microsofts_pc_game_pass/
- https://www.reddit.com/r/XboxGamePassPC/comments/n3nkxm/game_file_location/

## STEAM
This tool relies on [STEAM](https://store.steampowered.com)'s "libraryfolders.vdf" and "appmanifest_[steam_app_id].acf" files.

Those files seem to get updated when [STEAM](https://store.steampowered.com) is closed.

So there are two scenarios in which the tool incorrectly detects no [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation.
### 1st scenario
- [STEAM](https://store.steampowered.com) opened
- [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) still not installed and gets installed right now
- open this tool -> it's unable to detect [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation, cause the "libraryfolders.vdf" and "appmanifest_[steam_app_id].acf" files aren't updated


Solution: close this tool, close [STEAM](https://store.steampowered.com) and open this tool again

### 2nd scenario
- [STEAM](https://store.steampowered.com) opened
- [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) already installed
- [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) gets moved into another directory
- open this tool -> it's unable to detect [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation, cause the "libraryfolders.vdf" and "appmanifest_[steam_app_id].acf" files aren't updated


Solution: close this tool, close [STEAM](https://store.steampowered.com) and open this tool again

# Kown Issues
please take a look at [Issues](https://github.com/suluknumoh/TranslateCS2/issues)

# Documentation (somewhat like that)
## First-Start
- start this tool
- it creates an [SQLite](https://www.sqlite.org/)-Database called "Translations.sqlite"
- please read this page carefully
- each tab represents a view, you can navigate to
- each tab has a "Navigation"-Button; it's the first Toggle-Button on the left side (eventually on the right side for cultures with a flow-direction from right to left) and it indicates what's shown
- please take a look at Credits by switching to the Credits-Tab and clicking on the Navigation-Button
- please read the Credits carefully
- open the "Session(s)"-Tab
- click on the Navigation-Button to open the Session(s)-view
- click on create a new Translation-Session

## Creating a new Translation-Session/Editing an existing Translation-Session
- give your session a name : may not be empty or only whitespaces
- select a merge-file : an existing translation, you want to merge your translation with
- select an overwrite-file : i (the author of this app) tried to add non-existing ".loc"-files and the weren't recognized within [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii); so this should be a loc-file different to english (can't be selected) and (recommended) a language you are unable to read/speak; so you always have an ingame-fallback without the need to verify your installation files
- localename in english : should be selected via drop down
- localename localized : your desired localename in 'your' language; may be selected via drop down, i (the author of this app) do not know about restrictions/limitations

## Editing
- pressing enter or return moves to the next row
- just start typing within the "Translation"-Cloumn
- if you want to copy paste, press backspace on the respective cell within the "Translation"-Column
- since Translation-Cells accept return for multi-line-editing, cell editing can be stopped by pressing escape
- after adding a translation, all entries with the same value receive the translation and its saved to the [SQLite](https://www.sqlite.org/)-Database "Translations.sqlite"
- a right click on a row opens a context menu that provides the ability to edit the current entry in a new window
### Edit-View
- four columns are shown
1. Key
    - this column displays the key used by [Colossal Order](https://colossalorder.fi)
2. English Value
    - this column displays the value/text that corresponds to the key within the Key-column
    - it's always english, cause english seems to be the 'leading' language
3. Merge Value
    - displays the value/text that corresponds to the key within the Key-column inside the localization file to merge with that is selected while creating the translation session
4. Translation
    - the new value that is going to correspond to the respective key within the Key-column
    - for each cell that is left empty, the respective value/text within the Merge Value-column gets exported

### Edit by occurances-View
- four columns are shown
- Key-column is missing, cause each entry/value can relate to multiple keys
1. English Value
    - this column displays the value/text that corresponds to the key within the Key-column
    - it's always english, cause english seems to be the 'leading' language
2. Merge Value
    - displays the value/text that corresponds to the key within the Key-column inside the localization file to merge with that is selected while creating the translation session
3. Count
    - displays the count/amount/quantity of times the english value occurs/appears
    - a little more technical: the amount of keys the english value is used for
4. Translation
    - the new value that is going to correspond to the respective key within the Key-column
    - for each cell that is left empty, the respective value/text within the Merge Value-column gets exported

## Ex-Import
### Export
#### JSON
##### general
- to share your translations with others
- select a json-file to export to
- click export

##### TranslateCS2Mod
- you should check 'add key'
- you may check 'add merge values'
    - especially for those who like to:
        - create a new language, 'nl-NL' for example
        - and like to have the untranslated part in 'fr-FR', for example
- to use with [TranslateCS2.Mod](https://mods.paradoxplaza.com/mods/79187/Windows)
    - https://forum.paradoxplaza.com/forum/threads/translatecs2-translatecs2-mod.1669371/
    - https://github.com/suluknumoh/TranslateCS2/tree/main/TranslateCS2.Mod

##### I18NEverywhere
- compatibility is not guaranteed
- at least, you should NOT check 'add key'
- to use the translation with [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod
- please see also: https://forum.paradoxplaza.com/forum/threads/i18n-everywhere.1646597/
- feel free to share the translation with him for his Mod:
    - https://github.com/baka-gourd/I18NEverywhere.Localization

### Import
- to import shared translations that are exported with this tool
- select the json-file to read to review
- click read selected file
- the selected json-file is read
- afterwards a confrontation/juxtaposition/comparison is shown
- it shows three columns:
    1. Key
    2. Existing Translation
    3. Imported Translation
- there are three Import-Modes available
    1. NEW
        removes all existing translations and takes all imported translations
    2. LeftJoin
        keeps all existing translations and adds missing imported translations
    3. RightJoin
        takes all imported translations and keeps existing translations that aren't imported
- default Import-Mode is LeftJoin
- the colums Existing Translation and Imported Translation are highlighted with colors based on the selected Import-Mode
    1. light green
        indicates the translations that are kept
    2. light salmon
        indicates the translations that are removed/replaced
- after reviewing and selecting an appropriate Import-Mode, a click on import-button
    1. a backup of the database gets created
    2. translations are imported into the database

# Recommendations/Suggestions
- To re-export the translation(s) after [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) is updated
