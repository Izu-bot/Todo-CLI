# Todo CLI

<div align="center">
<img src="icon/list.png" alt="Icone" style="width:30%;position:center">
</div>

Todo CLI is a command line tool designed to make it easier to manage your daily tasks. With a clean and minimalist interface, it offers a distraction-free experience. Its simple and intuitive commands allow you to organize and control your activities efficiently.

## Commands

In total, it contains four basic commands

- create (add)
  - Create a new task
- remove (remove)
  - Remove a task
- list (list)
  - List your tasks
- update (update)
  - Update a task

### Steps to install the app

- Download the `todo.deb` package from this repository
- Open the directory where you downloaded the package
- The command `sudo dpkg -i todo.deb` will install the app
- Use the `todo` command to check that everything is working

#### WARNING

this app only works on Debian derivatives, it does not yet have a Windows version. Later, I'll implement a Flatpak version and a Windows version, and I'll make a documentation in PT-BR.

##### Specifications

This is version `1.0.0` so there will still be changes, mainly to how the `todos.json` file works. Currently it is generated in the repository where you downloaded the app.
