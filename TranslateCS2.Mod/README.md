# Disclaimer
This CodeMod for [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) is provided as is to use at your own risk.

Please see also: [MIT-License](https://github.com/suluknumoh/TranslateCS2?tab=MIT-1-ov-file)

# Intention (in addition to the main-project)
To load custom translations that are listed within [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)'s User Interface.

# How it works
## preamble
- the engine, [Unity](https://unity.com),  used by [Colossal Order](https://colossalorder.fi), supports a general set of languages
    - for example:
        - locales/flavours like
            - en-GB
            - en-US
            - and others; in general 'en-'
        - result in [Unity](https://unity.com)'s language 'English'
    - this general support limits the options for sideloading additional languages or flavors
- [Colossal Order](https://colossalorder.fi) already provided us, the community, with a rich set of translations
    - de-DE
    - en-US
    - es-ES
    - fr-FR
    - it-IT
    - ja-JP
    - ko-KR
    - pl-PL
    - pt-BR
    - ru-RU
    - zh-HANS
    - zh-HANT
- for those languages, only flavors can be added

### general
- create a JSON-file or use an existing ones
- the JSON-file should contain one JSON-Object
- the JSON-Object should contain commaseperated Key-Value-Pairs
- use [Colossal Order](https://colossalorder.fi)'s respective Key as Key and the custom translation as Value
- filename format:
    - [language-code]-[region].json
    - example:
        - en-US.json

#### example

```
{
  "TranslateCS2.LocaleNameLocalizedKey": "Nederlands",
  "Options.SECTION[General]": "Algemeen"
}
```


### 1st Use-Case: a new flavour
- a complete new flavour like British English ('en-GB'), for example
- create a JSON-file or use an existing ones, as described above
- name it 'en-GB.json' and put it into this mods directory
- after [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) is started, nothing is applied automatically
    - because 'en-GB' results in [Unity](https://unity.com)'s language 'English'
- open the Options-Menu within [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)'s Main-Menu
- there is a new entry with this Code-Mods name: 'TRANSLATECS2'
- after selecting this entry, there are a few options available
    - behaviour
        - add as source to existing
            - hovering over it, displays the following description:
                - Unity SystemLanguages are rare and general. For example: 'en-US' and 'en-GB' result in Unitys SystemLanguage 'English'. By checking 'add as source to existing', 'en-GB.json' would be added as source to 'en-US' and 'overwrite' the existing texts with the provided texts. As far as the respective json file existed on startup, a click on 'reload language files' does the trick. can be reverted with a click on 'clear overwritten'.
        - to use the language-flavour, dropped into this mods directory, check this check-box
        - a click on 'reload language files' adds the flavour to, in this example, English
    - clear overwritten
        - removes eventually added flavours from, in this example, English
    - reload
        - reload language files
            - all language files, that are known to this mod are reloaded, if they still exist
            - allows you to change translations more or less on the fly, without the need to restart [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
            - reactivates a flavour, if 'add as source to existing' within the 'behaviour'-group is checked

#### example
```
{
  "Options.SECTION[General]": "General (british)"
}
```


### 2nd Use-Case: a complete new language
- a complete new language like Dutch ('nl-NL'), for example
- create a JSON-file or use an existing ones, as described above
- an additional Key-Value-Pair, `"TranslateCS2.LocaleNameLocalizedKey": "Nederlands"`, for example, lets you control what's shown within the language-selection
    - otherwise the native name is used
- name it 'nl-NL.json' and put it into this mods directory
- after [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) is started, nothing is applied automatically
    - now the complete new language is listed within the selectable languages within the interface-settings
- after opening the Options-Menu within [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)'s Main-Menu
- there is a new entry with this Code-Mods name: 'TRANSLATECS2'
- after selecting this entry, there are a few options available
    - behaviour
        - only for the first Use-Case
    - clear overwritten
        - only for the first Use-Case
    - reload
        - reload language files
            - all language files, that are known to this mod are reloaded, if they still exist
            - allows you to change translations more or less on the fly, without the need to restart [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)

#### example

```
{
  "TranslateCS2.LocaleNameLocalizedKey": "Nederlands",
  "Options.SECTION[General]": "Algemeen"
}
```

# limitations
as described in the preamble of how it works:

- for the built-in languages, only flavors can be added
    - de-DE
    - en-US
    - es-ES
    - fr-FR
    - it-IT
    - ja-JP
    - ko-KR
    - pl-PL
    - pt-BR
    - ru-RU
    - zh-HANS
    - zh-HANT
- adding multiple language files, that result in a single [Unity](https://unity.com)-language:
    - flavors are added in the order of load

