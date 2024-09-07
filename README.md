# <b>[Witcher Guessr](https://synbox.dev/Projects/Preview?project=WitcherGuessr)</b>

## 📄 <b>Table of contents</b>

* [About the project](#about-the-project)
* [Getting started](#getting-started)
    * [If you wish to just simply play the game](#if-you-wish-to-just-simply-play-the-game)
    * [If you wish to actually inspect the project itself](#if-you-wish-to-actually-inspect-the-project-itself)
* [Contributing](#Contributing)
* [Technology stack](#technology-stack)
* [Roadmap](#roadmap)
* [Credits & Legalities](#credits--legalities)

## <b>About the project</b>

'Witcher Guessr' is an unofficial map location guessing game, based entirely on [The Witcher 3: Wild Hunt](https://www.thewitcher.com) made by [CD Projekt Red](https://www.cdprojektred.com).

Core gameplay loop revolves around carefully inspecting a 360° degree panorama image of a randomly selected place in the game world, and then accurately guessing its precise location in one of many maps from the game world.

Project is, and will for ever remain free and open-source, as an appreciation and as a nod to the talented team who have have brought The Witcher universe to life.

---

## <b>Getting started</b>

### <b>If you wish to just simply play the game</b>

You can do so, by simply visiting [project's page on my portfolio website](https://synbox.dev/Projects/Preview?project=WitcherGuessr), and playing it to your heart's content.

### <b>If you wish to actually inspect the project itself</b>

Follow the steps described below:

1. First, and foremost, you'll have to download & install the Unity Engine itself. You can do so by visiting their official [download page](https://unity.com/download).

2. Second of all, you must first clone the repository (download the project to your own machine). You can do this by a number of methods, here's two:

    * Method 1 - Downloading zipped repository and extracting it locally:
[Download](https://github.com/synboxdev/WitcherGuessr/archive/refs/heads/main.zip) zipped repository, and extract the files to a folder of your choosing.

    * Method 2 - Using [Windows CLI](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd) to download the repository:
    1. Open up <b>Cmd.exe</b>
    2. Navigate to an empty folder of your choosing
    <br><br>
    ```
    cd C:\Users\DeveloperUser\Desktop\MyEmptyFolder
    ```
    3. Execute a command that clones (downloads a copy) the repository to your current directory
    <br><br>
    ```
    git clone https://github.com/synboxdev/WitcherGuessr.git
    ```

3. Next, you can now simply open the project with Unity Engine via Unity Hub by [Opening existing project](https://docs.unity3d.com/2021.1/Documentation/Manual/GettingStartedOpeningProjects.html). Press 'Open' in Unity Hub, and in File Explorer navigate inside the extracted folder (which will be named 'WitcherGuessr-main'), and select 'WitcherGuessr' folder to be opened.

4. <b>[TBD]</b> Project currently will NOT function, since Maps and Location (image files) are currently excluded from the project's repository, and they are simply 'missing' from it. So Unity will be confused since you'd be attempting to 'load' in files that technically do not exist in the project. This will be looked into. Look at [roadmap](#roadmap) section of this document.

Best of luck!

---

## <b>Contributing</b>

Generally speaking, any and all positive contributions are welcome, including features, bugfixes, or new functionalities. See [contributing documentation](CONTRIBUTING.md) for more details.

---

## <b>Technology stack</b>

Technologies used for the development of the game project itself:

* #### [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) 
* #### [Unity Engine](https://unity.com/)
* #### [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) / [Visual Studio Code](https://code.visualstudio.com/)
* #### [Git](https://learn.microsoft.com/en-us/devops/develop/git/what-is-git) / [Sourcetree](https://www.sourcetreeapp.com/)

Technologies used for creating in-game panorama images (A terrible way of doing it. Reasoning and the process behind it are explained below):

* #### Modded [The Witcher 3: Wild Hunt](https://www.thewitcher.com)
* #### [Nvidia Ansel ](https://www.nvidia.com/en-us/geforce/geforce-experience/ansel/) interface

---

## <b>Roadmap</b>

At the time of writing this (2024-09), project recently underwent a significant overhaul - panoramic image viewing functionality has been refactored (among other things), since I've started using [Nvidia Ansel ](https://www.nvidia.com/en-us/geforce/geforce-experience/ansel/) interface, which allows for <i>real</i> panoramic images - spheric, 360 degree images, instead of a 'pseudo' panoramic, stitched together, horizontal images that I've used in the past.

Because of that - I've outlined the following roadmap for the project's future, and its development. It will most likely be changed and adjusted over time. That being said - here's the outlook:
	
- Highest priority:
    - Add locations from the entire game to the project. (Including playthrough-only locations, probably mod in a 100% save for this)
	
- Medium priority:
	- Find a way to store map and location image files outside of repository - because Git LFS sucks. (Probably a downloadable archive from portfolio website) + Update '[Getting started](#getting-started)' section of this document
	- QoL changes (Ex. Location viewing image drag sensitivity setting, disable addressable caching setting)
	- Create a Desktop build of the project and upload it to Itch.io and make downloadable link directly on portfolio website

-  Lower priority:
	- (Continuously) Update project's repository documentation
    - Look into creating a Linux/Mac builds, and uploading them either to Itch.io and/or Portfolio website
	- Maybe make an updated 'gameplay' video/trailer?

Think of this roadmap as more like guidelines, rather than guaranteed promises. A public to-do list, if you will. :)

---

## <b>Credits & Legalities</b>

As mentioned in the introductory description, this is an unofficial project, completely unrelated to [CD Projekt Red](https://www.cdprojektred.com). All assets related to The Witcher 3, logos, icons, maps, and in-game panorama images are property of [CD Projekt Red](https://www.cdprojektred.com) and were used without permission. The project was developed with the best efforts to follow the official [CD Projekt Red - Fan content guidelines](https://www.cdprojektred.com/en/fan-content) and the official [CD Projekt Red - User agreement](https://regulations.cdprojektred.com/en/user_agreement).

Special thanks and credit where credit is due to:
* [CD Projekt Red](https://www.cdprojektred.com) and the team behind the masterpiece that is [The Witcher 3: Wild Hunt](https://www.thewitcher.com) - for creating a game that is beloved by many.
* [Andrzej Sapkowski](https://en.wikipedia.org/wiki/Andrzej_Sapkowski) - for creating The Witcher universe.
* Modding community of [The Witcher 3: Wild Hunt](https://www.thewitcher.com) and [Nexus Mods](https://www.nexusmods.com/) - for creating and hosting, respectively, a plethora of fantastic mods that eased the development process of this project, as well as enhanced gaming experience.
* [GeoGuessr](https://www.geoguessr.com/) and [Where in Warcraft?](https://www.whereinwarcraft.net/) - for inspiration for the project itself.