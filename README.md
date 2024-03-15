# ConsoleDatabase
Simple console database that allows storing, editing, and managing account credentials and notes  directly from the command line interface.
Simple console database that allows storing, editing, and managing account credentials and notes directly from the command line interface. This application is particularly useful for users who manage multiple accounts that frequently encounter bans. It enables users to track the status of each account, distinguishing between available and unavailable (banned) accounts, quickly fetch an available account, and view the duration of any bans.

## Description of Commands and Functionality

### General Menu
- **0** - Display menu: Shows the list of available commands.
- **1** - Choose a random account from the database: Selects an account at random that is not marked as banned.
- **2** - Choose an account based on a group: Allows selecting an account from a specific group.
- **3** - Mark account as unavailable (banned): Marks a selected account as banned and prompts for the duration of the ban.
- **4** - Select account by the name: Fetches an account details by entering the account's name.
- **5** - Add a new account to the database: Adds a new account with details including name, password, group, and optional notes.
- **6** - Modify account by name: Allows modification of an account's details by specifying its name.
- **7** - Modify the current selected account: Modifies details of the account currently selected or viewed.
- **8** - Display statistics: Shows statistics including the total number of accounts, the number of accounts in each group, and how many are available vs. banned.
- **9** - Print all accounts: Lists all accounts in the format: Name:Pass | Group | Notes (if any) | Ban duration (if banned).
- **55** - Bulk add accounts: Enables adding multiple accounts in the Name:Password format, useful for importing accounts of the same group.
- **111** - Delete account from the database: Removes a specified account from the database.

### Account Edit Menu
- **0** - Go back to the main menu: Returns to the list of general commands.
- **1** - Edit password: Changes the password of the currently selected account.
- **2** - Edit group: Modifies the group of the current account.
- **3** - Mark account as available (unban): Removes the ban status from the selected account.
- **4** - Remove account notes: Deletes any notes associated with the account.
- **5** - Add notes to account: Allows adding notes to the selected account for additional information.

## Development Tools and IDE

The application was developed in C# language using Visual Studio 2022. For data serialization to and from the database in a .json format, the solution leverages the NewtonSoft.Json dependency, which is connected through NuGet packages.