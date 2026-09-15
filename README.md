# Password Hashing CLI

Aplicação de linha de comando para gerar hashes de senha com BCrypt ou ASP.NET Identity e identificar o tipo de um hash existente.

## Requisitos

- .NET SDK 10

## Compilar

```powershell
dotnet restore
dotnet build -c Release
```

## Gerar um hash

Informe a senha e o tipo de hasher desejado:

```powershell
dotnet run --project .\src\password-hasing.Cli -c Release -- hash --password "minha-senha" --hasher-type BCrypt
```

Tipos suportados:

- `BCrypt`
- `AspNet` — ASP.NET Identity

Exemplo de saída:

```text
Hasher type: BCrypt
Password hash: $2a$...
```

## Usar appsettings.json

Configure a senha e o tipo padrão em [appsettings.json](src/password-hasing.Cli/appsettings.json):

```json
{
  "PasswordHashing": {
    "Password": "minha-senha",
    "HasherType": "BCrypt"
  }
}
```

Em seguida, execute:

```powershell
dotnet run --project .\src\password-hasing.Cli -c Release -- hash
```

Os valores informados na linha de comando substituem individualmente os valores do `appsettings.json`:

```powershell
dotnet run --project .\src\password-hasing.Cli -c Release -- hash --hasher-type AspNet
```

## Identificar um hash

```powershell
dotnet run --project .\src\password-hasing.Cli -c Release -- identify --hash '$2a$11$...'
```

No PowerShell, use aspas simples para hashes BCrypt, pois o caractere `$` tem significado especial.

Exemplo de saída:

```text
Hasher type: BCrypt
```

## Ajuda e códigos de saída

```powershell
dotnet run --project .\src\password-hasing.Cli -c Release -- --help
dotnet run --project .\src\password-hasing.Cli -c Release -- hash --help
dotnet run --project .\src\password-hasing.Cli -c Release -- identify --help
```

A aplicação retorna `0` em caso de sucesso e `1` para parâmetros inválidos, configuração incompleta ou hash não reconhecido.

## Segurança

Não versione senhas reais no `appsettings.json`. Senhas passadas pela linha de comando podem ficar expostas no histórico do terminal e na lista de processos; para ambientes compartilhados, prefira uma fonte de configuração protegida, como variáveis de ambiente:

```powershell
$env:PasswordHashing__Password = "minha-senha"
$env:PasswordHashing__HasherType = "BCrypt"

dotnet run --project .\src\password-hasing.Cli -c Release -- hash
```
