# early adopters (currently)
- project TranslateCS2 can not be published as single-file anymore
- what ever you do, be very careful with your API-Key(s), it/they can be gatherd by reverse-engineering!!!
- if you plan to share your Translator-Module or a rebuild of this App that includes your Translator-Module, you should ensure the Auth-Key is read from an external file, so each user is able to enter his/her personal Auth-Key
- take care of
    - line breaks (multiline-texts)
    - variables within texts (as far as I know, they are enclosed by curly braces: `{variablename}`)
    - control characters like the double asterix `**` (there may be more/others)

## getting started in Visual Studio
- there are two solutions available
    - `TranslateCS2.sln`
        - preferably for those who want to (re-)build the whole app
    - `TranslateCS2_TranslatorsExample.sln`
        - a quick example and overview
    - create a new solution
        - preferably for those who only want to implement/realize a translator as a drop-in module
- choose your preferred ones or create a new solution
    - if a new solution is created, add the existing project `TranslateCS2.Core` to it
- add a new project to the solution
    - choose `WPF Class Library`
- give it a name, for example: `TranslateCS2.TranslatorMyPreferredTranslatorAPI`
- choose `.NET 8.0` as Framework
- now edit the project file
    1. change `TargetFramework` from `net8.0` to `net8.0-windows`
    2. add the following:
        - `<Import Project="../TranslateCS2.Core/BuildTargets/Translator.Build.targets"/>`
- add a Project Reference to the newly created Project
    - only select `TranslateCS2.Core`
- remove the existing Class1.cs
- add a new Class, for example: `MyPreferredTranslatorAPI`
    - this may be an internal class
    - let this class extend `TranslateCS2.Core.Translators.ATranslator`
    - implement and realize the inherited methods
        - `Task InitAsync(HttpClient httpClient)`
            - ensure to add target language codes to the TargetLanguageCodes Property of this class
        - `Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s)`
            - the selected target language code can be gathered from the `SelectedTargetLanguageCode` Property of this class
            - handle multiline text if needed
            - do one or more requests to the preferred translator API
            - whatever is needed to use the API and to translate
            - take care of
                - line breaks (multiline-texts)
                - variables within texts (as far as I know, they are enclosed by curly braces: `{variablename}`)
                - control characters like the double asterix `**` (there may be more/others)
- now, add a new Class, for example: `TranslatorMyPreferredTranslatorAPIModule`
    - this class has to be public
    - let this class extend `TranslateCS2.Core.Translators.Modules.ATranslatorModule`
    - add the following class attribute
        - `[ModuleDependency(nameof(CoreModule))]`
        - it's `Prism.Modularity`'s `ModuleDependencyAttribute`
        - with the nameof `TranslateCS2.Core.CoreModule`
        - it expresses that the module depends on `TranslateCS2.Core.CoreModule`
        - so the used [Prism Library](https://www.prismlibrary.com) knows to load the `CoreModule` first
    - add the `MyPreferredTranslatorAPI` to the `Translators` Property of this class
## Testing
- add a new project to the solution
    - choose `xUnit Test Project`
- give it a name, for example: `TranslateCS2.TranslatorMyPreferredTranslatorAPI.Tests`
- add a Project Reference to the newly created `xUnit Test Project`
    - only select `TranslateCS2.TranslatorMyPreferredTranslatorAPI`
- if your `TranslateCS2.TranslatorMyPreferredTranslatorAPI.MyPreferredTranslatorAPI` class is an `internal` ones and you don't want to make it `public`
    1. go back to `TranslateCS2.TranslatorMyPreferredTranslatorAPI`
    2. add a class called `AssemblyInfo`
    3. add `[assembly: InternalsVisibleTo("TranslateCS2.TranslatorMyPreferredTranslatorAPI.Tests")]`
- and go on


## run/debug within app

### the `(re-)build the whole app`-way
- add a Project Reference to `TranslateCS2` to `TranslateCS2.TranslatorMyPreferredTranslatorAPI`
- open `TranslateCS2.App.xaml.cs`
- take a look at `void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)`
- add `TranslatorMyPreferredTranslatorAPI` via `moduleCatalog.AddModule<TranslatorMyPreferredTranslatorAPI>();` to the `moduleCatalog`
- see also: [Prism Library Modularity](https://docs.prismlibrary.com/docs/modularity/index.html)
- see also: [Prism Library Modules Code](https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Code)
- and go on

### the `drop-in module` way
- (re-)build `TranslateCS2.TranslatorMyPreferredTranslatorAPI`
- open the output directory
- copy all files
- open the location of `TranslateCS2.exe`
- create a new directory named `modules`
- open the newly created directory `modules`
- paste the copied files
- see also: [Prism Library Modularity](https://docs.prismlibrary.com/docs/modularity/index.html)
- see also: [Prism Library Modules Directory](https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Directory)
- and go on

#### directory structure
- App-Directory
    - `modules`-Directory
        - eventually libraries from used nuget packages
            - for `TranslateCS2.TranslatorsExample` it is `DeepL.net.dll`
        - `TranslateCS2.TranslatorMyPreferredTranslatorAPI.dll`
        - `TranslateCS2.TranslatorMyPreferredTranslatorAPI.deps`
        - `TranslateCS2.TranslatorMyPreferredTranslatorAPI.pdb`
    - `TranslateCS2.exe`
    - `Translations.sqlite`
    - backups of `Translations.sqlite`
    - other files


