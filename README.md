# <b>[Witcher Guessr](https://synbox.itch.io/witcher-guessr)</b>

## 📄 <b>Table of contents</b>

* [About the project](#about-the-project)
* [Why Itch.io?](#why-itchio)
* [Getting started](#getting-started)
    * [If you wish to just simply play the game](#if-you-wish-to-just-simply-play-the-game)
    * [If you wish to actually inspect the project itself](#if-you-wish-to-actually-inspect-the-project-itself)
* [Contributing](#Contributing)
* [Technology stack](#technology-stack)
* [Credits & Legalities](#credits--legalities)

---

## <b>About the project</b>

'Witcher Guessr' is an unofficial map location guessing game, based entirely on [The Witcher 3: Wild Hunt](https://www.thewitcher.com) made by [CD Projekt Red](https://www.cdprojektred.com).

Core gameplay loop revolves around carefully inspecting a 360° degree panorama image of a randomly selected place in the game world, and then accurately guessing its precise location in one of many maps from the game world.

Project is, and will for ever remain free and open-source, as an appreciation and as a nod to the talented team who have have brought The Witcher universe to life.

---

## <b>Why Itch.io?</b>

Initially, the idea was to host this project directly here, on [GitHub Pages](https://pages.github.com/), how ever the [usage limits](https://docs.github.com/en/pages/getting-started-with-github-pages/about-github-pages#usage-limits) of GitHub pages makes this practically impossible, since build size of the project, primarily impacted by file sizes of high quality map images, and panorama images themselves exceeds the limits by a good amount.

That's where [Itch.io](https://itch.io/docs/general/about) comes into play - free to use, open marketplace which allowed me to host this project basically without much of a barrier of entry. I've still wanted to host this as a web-based project, so that ease of access remains, how ever [requirements and limitation for HTML5 games](https://itch.io/docs/creators/html5#zip-file-requirements) made this impossible.

That's why, the project currently is, and will likely remain as a downloadable executable project. After any and all changes made to the project in this repository, the project will be updated in Itch.io page as well.

---

## <b>Getting started</b>

### <b>If you wish to just simply play the game</b>

You can do so, by simply visiting [project's page on Itch.io](https://synbox.itch.io/witcher-guessr), downloading the game for free and playing it at your heart's content, or you could download it directly here, on GitHub, by visiting [releases section](https://github.com/synboxdev/WitcherGuessr/releases) and downloading the latest version of the project!

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

4. Project will open up, and you'll be able to start it and inspect it inside the Unity Editor. 

Best of luck!

---

## <b>Contributing</b>

Probably the most lacking aspect of this project is the lack of panorama images (locations) and their quality. That's where you, the community, can easily help out!

On the off chance that you own an NVIDIA graphics card (which I unfortunately do not) - you could probably use [NVIDIA Ansel](https://www.nvidia.com/en-us/geforce/geforce-experience/ansel/) which fortunately supports [The Witcher 3: Wild Hunt](https://www.nvidia.com/en-us/geforce/geforce-experience/games/) and could be used to take high quality, 360° degree panorama images that could be added to this project. I have not had a chance to try this piece of software, but by the looks of it, it could be extremely useful to the development of this project. 

Generally speaking, any and all positive contributions are welcome, including features, bugfixes and of course - panorama images themselves. See [contributing documentation](CONTRIBUTING.md) for more details.

---

## <b>Technology stack</b>

Technologies used for the development of the game project itself:

* #### [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) 
* #### [Unity Engine](https://unity.com/)
* #### [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) / [Visual Studio Code](https://code.visualstudio.com/)
* #### [Git](https://learn.microsoft.com/en-us/devops/develop/git/what-is-git) / [Sourcetree](https://www.sourcetreeapp.com/)

Technologies used for creating in-game panorama images (A terrible way of doing it. Reasoning and the process behind it are explained below):

* #### Modded [The Witcher 3: Wild Hunt](https://www.thewitcher.com)
* #### [Open Broadcaster Software](https://obsproject.com/)
* #### [Microsoft Image Composite Editor](https://www.microsoft.com/en-us/research/project/image-composite-editor/)

### <b>Current process of creating panorama images ..</b>

1. Find a suitable in-game location for a panorama image to be taken in.
2. Utilize console commands, that would allow for uninterrupted 360° degree panorama recording to be taken. Commands that hide the UI, hide player entity, pauses (freezes) the game world and possibly some commands to alter the weather or time of day.
3. Using OBS (or whatever recording software of your choice), record an uninterrupted, 360° degree, ~20-30s long, panorama video, moving your in-game camera ideally in completely horizontal way.
4. Use [Microsoft Image Composite Editor](https://www.microsoft.com/en-us/research/project/image-composite-editor/) in-built feature that allows to create a panorama from a panning video.
5. Export the image to highest possible quality format. I've used .TIFF format, which seemed to output the highest quality that the software could produce (which is still rather low quality, but that's the best it could do).
6. Import the image as a Sprite to the project itself in Unity Engine. Adjust import settings to disable image compression.

### <b>.. and why you probably should not do it the same way</b>

As you might already tell from the few points mentioned above - the process of creating a panorama image this way is unnecessarily complicated and troublesome. Here's a few reasons why:
1. <b>Creating each individual panorama takes way too much time.</b> Depending on the speed of your workflow, and performance of your machine, each attempt at creating the panorama image (recording a 360° degree video), processing it with [Microsoft Image Composite Editor](https://www.microsoft.com/en-us/research/project/image-composite-editor/) might take upwards of 5-10 minutes. And realistically, it takes probably around 20-30 attempts per location, for you to end up with at least an 'OK' quality panorama image.
2. <b>Most processed panorama images by [Microsoft Image Composite Editor](https://www.microsoft.com/en-us/research/project/image-composite-editor/) end up being 'glitched'.</b> To elaborate - overlapping frames being distorted, objects being multiplied or faded. This roots back into the first point, because of many many repeated attempts being necessary.
3. <b>Quality of the final panorama image is still bad.</b> Overall image quality still remains low, almost regardless of in-game graphics quality settings, recording software settings for quality and framerate, and limited configuration and settings of the [Microsoft Image Composite Editor](https://www.microsoft.com/en-us/research/project/image-composite-editor/).

---

## <b>Credits & Legalities</b>

As mentioned in the introductory description, this is an unofficial project, completely unrelated to [CD Projekt Red](https://www.cdprojektred.com). All assets related to The Witcher 3, logos, icons, maps, and in-game panorama images are property of [CD Projekt Red](https://www.cdprojektred.com) and were used without permission. The project was developed with the best efforts to follow the official [CD Projekt Red - Fan content guidelines](https://www.cdprojektred.com/en/fan-content) and the official [CD Projekt Red - User agreement](https://regulations.cdprojektred.com/en/user_agreement).

Special thanks and credit where credit is due to:
* [CD Projekt Red](https://www.cdprojektred.com) and the team behind the masterpiece that is [The Witcher 3: Wild Hunt](https://www.thewitcher.com) - for creating a game that is beloved by many.
* [Andrzej Sapkowski](https://en.wikipedia.org/wiki/Andrzej_Sapkowski) - for creating The Witcher universe.
* Modding community of [The Witcher 3: Wild Hunt](https://www.thewitcher.com) and [Nexus Mods](https://www.nexusmods.com/) - for creating and hosting, respectively, a plethora of fantastic mods that eased the development process of this project, as well as enhanced gaming experience.
* [GeoGuessr](https://www.geoguessr.com/) and [Where in Warcraft?](https://www.whereinwarcraft.net/) - for inspiration for the project itself.