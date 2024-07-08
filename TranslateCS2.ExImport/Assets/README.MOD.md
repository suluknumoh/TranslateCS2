# 1. TranslateCS2.Mod

This mod is intended to sideload custom localizations/translations from JSONs.

It does not compete with [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod.

Users have to pro-actively select a different flavor, especially for built-in languages.

# 2. Disclaimer
This mod is provided as is to use at your own risk.

Please see also: [MIT-License](https://github.com/suluknumoh/TranslateCS2?tab=MIT-1-ov-file)



Please take a look at section '3.1.3. more how it works explanation' within this document.


# 3. Guides
## 3.1. general

### 3.1.1. JSON-File-Format

* a .json-file
* should contain a single Object
* this Object should have commaseparated Key-Value-Pairs
* the Key should be the Key that is used by Colossal Order
* the Value should be the translation
* should be UTF8-encoded
* example:

```
{
"TranslateCS2.LocaleNameLocalizedKey": "Nederlands",
"Options.SECTION[General]": "Algemeen"
}
```

* the example above contains the Key-Value-Pair "TranslateCS2.LocaleNameLocalizedKey": "Nederlands"
* that Key is not needed and may be left
* if that Key-Value-Pair is missing, the native name is gathered from the supported CultureInfo that matches the filename and is shown within the drop down to select a flavor
* if the Key-Value-Pair is present and its Value is not left empty, its Value is shown within the drop down to select a flavor

* the example above contains the Key "Options.SECTION[General]": "Algemeen"
* please take a look at '3.1.2. working-hierarchy' within this document

### 3.1.2. working-hierarchy
* s long as no other mod is loaded after this one and overwrites the values and as long as no other mod overwrites the corresponding methods to return other values
* translation(s) provided to this mod, as described within the following section, rule and wont get overwritten by mods described in section '3.3. to create a mod'
    * '3.2. to use your own translation for yourself'
    * '3.2.1. JSON-File-Naming and Location'
* if a mod described in section '3.3. to create a mod' is installed and active within the current playset and it contains more/other/additional translations, those translations are taken into account

#### 3.1.2.1. built-in languages flavor 'none' selected
* built-in Values/Texts/Translations are used

#### 3.1.2.2. built-in languages and a provided flavor is selected
* if a 'new' Value/Text/Translation is available within the specific provided translation-file, that Value/Text/Translation is used
* otherwise the Value/Text/Translation provided by Colossal Order is used


#### 3.1.2.3. new non built-in languages
* only languages, a respective language file is provided, are available within the language drop down in the default interface-settings
* after selecting such a new non built-in language within the default interface-settings, the first found flavor is applied
* if there is only one flavor provided, everything should be fine
* if there are multiple flavors provided, the desired ones can be selected within the 'TRANSLATECS2'-Menu
* flavor drop downs do not contain 'none' as selectable item


* if a new Value/Text/Translation is available for a specific Key within the specific provided and selected translation-file (flavor), that Value/Text/Translation is used
* otherwise Colossal Orders Value/Text/Translation for that specific Key is taken from Colossal Orders fallback-language ('en-US', as far as i know/gathered) and used


### 3.1.3. more how it works explanation
* this mod knows your previous selected language
* this mod also knows your selected language while this mod is active
* this mod also knows your selected flavor while this mod is active
* at the latest when the game is left those settings are saved
* as soon as the game is exited, the previous selected language or, if unknown for whatever reason, the language of the used operating system is set to the default interface-settings to grant specific game experience if the game is started without this mod next time
* this mod scans a specific directory within other mods that are enabled in the current playset to detect transslation(s)
    * this mod forgets scanned mods; it does not save mod information
        * mod information are only used within possible error messages, to be more precise, what causes the error
    * if you feel uncomfortable with it, this mods allows you to deactivate this functionality within this mods settings within the game
        * simply uncheck/disable 'Load from other mods' it is checked/enabled by default


## 3.2. to use your own translation for yourself
### 3.2.1. JSON-File-Naming and Location
* example: name it 'af-ZA.json' for 'Afrikaans (Suid-Afrika)'
* a list of supported language-/language-region-codes can be found in the paragraph 'Supported language and language-region-codes with their English names' within this document
* the .json-file should be placed into the following directory:
%AppData%\\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\


## 3.3. to create a mod
### 3.3.1. without logic that shares your translation with others
* this guide/section does not cover the basics
* therefore please visit https://cs2.paradoxwikis.com/Modding
* from my point of view, i recommend to also join the official [Cities: Skylines Modding](https://discord.gg/HTav7ARPs2)-Discord
* while creating a new Mod-Project based on [Colossal Order](https://colossalorder.fi)'s Mod-Template within Visual Studio
* and if you only want to provide your translation, there is no need to select/check/enable neither 'Include mod settings' nor 'Include key bindings'
* the Mod-class can/should look like the following:
```
    public class Mod : IMod {
        public void OnLoad(UpdateSystem updateSystem) { }
        public void OnDispose() { }
    }
```
* it should contain a directory called 'UnofficialLocales'
* this directory should contain the translation(s)
* example: name it 'af-ZA.json' for 'Afrikaans (Suid-Afrika)'
* a list of supported language-/language-region-codes can be found in the paragraph 'Supported language and language-region-codes with their English names' within this document
* ensure the directory called 'UnofficialLocales' is copied to out-dir
* ensure to add a dependency tag to 'PublishConfiguration.xml'
* please take care,
    * this mod allows users to disable loading translations from other mods
    * especially, if you would like to use this mod for your mods translation(s)
        * for built-in languages, users have to pro-actively select a different flavor
        * users are able to deactivate this mod
        * users are able to skip this mods installation
        * i, the author of this mod, recommend to use [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod for those purposes


### 3.3.2. to use this mod for your mods translations
* it should contain a directory called 'UnofficialLocales'
* this directory should contain the translation(s)
* example: name it 'af-ZA.json' for 'Afrikaans (Suid-Afrika)'
* a list of supported language-/language-region-codes can be found in the paragraph 'Supported language and language-region-codes with their English names' within this document
* ensure the directory called 'UnofficialLocales' is copied to out-dir
* ensure to add a dependency tag to 'PublishConfiguration.xml'
* please take care,
    * this mod allows users to disable loading translations from other mods
    * especially, if you would like to use this mod for your mods translation(s)
        * for built-in languages, users have to pro-actively select a different flavor
        * users are able to deactivate this mod
        * users are able to skip this mods installation
        * i, the author of this mod, recommend to use [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod for those purposes


# 4. Supported language and language-region-codes with their English names
* the following languages are supported with the listed country and/or country-region code
* due to technical limitations, country and/or country-region codes are limited
* full list can be found over there: https://github.com/suluknumoh/TranslateCS2/blob/main/TranslateCS2.Mod/LANGUAGES.SUPPORTED.md

# 5. Support for this Mod
you are welcome to leave concerns, issues, feedback and/or suggestions on the following platforms

## 5.1. Cities: Skylines Modding Discord
* feel free to join [Cities: Skylines Modding](https://discord.gg/HTav7ARPs2)-Discord
* this mod has a channel within it (https://discord.com/channels/1024242828114673724/1256524100462055445)

## 5.2. GitHub
* [TranslateCS2 - Issues](https://github.com/suluknumoh/TranslateCS2/issues)

# 6. Credits

## i286-1
- https://github.com/i286-1
- for:
    - testing and giving feedback
        - https://github.com/suluknumoh/TranslateCS2/issues/17

## yenyang
- https://github.com/yenyang
- for:
    - 'working' together
    - inspiration
    - testing
    - feedback

## Morgan · Toverux
- for:
    - 'working' together
    - inspiration
    - testing
    - feedback
