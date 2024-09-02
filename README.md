# PontoCerto.Api

## Instruções para Executar o Sistema

Siga os passos abaixo para configurar e executar o sistema localmente:

1. **Baixar o Arquivo**  
   Faça o download do repositório como arquivo `.zip`.

2. **Abrir a Solução**  
   Extraia o conteúdo do `.zip` e abra a solução no Visual Studio.

3. **Executar Migrations**  
   Abra o `Package Manager Console` no Visual Studio e execute o seguinte comando para aplicar as migrations e configurar o banco de dados:

   ```bash
   dotnet ef database update --project PontoCerto.Infrastructure --startup-project PontoCerto.Api
  ````

4. Executar a Aplicação
Execute a aplicação utilizando o perfil https.
