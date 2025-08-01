
# Installazione del tool per generazione API

# Posizionarsi nella cartella del progetto `AlternativeFuel.Client` e lanciare i seguenti comandi:

Installazione

# Downloads dotnet's project specific tooling file 
dotnet new tool-manifest 
# Downloads dotnet's Nswag CLI tool 
dotnet tool install NSwag.ConsoleCore 

Esecuzione
A terminale aperto su Client digitare .\BuildSdkClient.cmd (su poweshell) altrimenti sul prompt DOS digitare BuildSdkClient.cmd
ogni volta che viene aggiornato il backend


https://stenbrinke.nl/blog/openapi-api-client-generation/

Se dovesse apparire l'avviso che dice che dotnet-tools.json è bloccato per sicurezza fare
dotnet tool install --global Nswag.ConsoleCore
nswag run Swagger.nswag

Chiamate parallele
        private async Task GetFakeRecords()
        {
            var task1 = Task.Delay(3000);
            var task2 = Task.Delay(4000);
            var task3 = Task.Delay(5000);
            await Task.WhenAll(task1, task2, task3);
        }

Usare tag html nel Localizer

@((MarkupString)Localizer[ResourceLanguage.HelpHeader].Value)

Trasformare un Task in un EventCallBack

OnClick="@EventCallback.Factory.Create(this, MioMetodo)"