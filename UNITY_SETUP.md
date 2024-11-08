# <b>Setting up the project 'Witcher Guessr' in Unity Engine</b>

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

4. Next step that we'll have to do, is download the map and the location files, which are currently missing from the project, and I've got them hosted on a few different cloud storage providers. Reason for it is simple - GitHub has very limited and very restrictive storage options, and maps and locations, at the time of writing this, sum up to ~23.5 GB, which is fairly significant.

Download the maps, and locations, from one of mirror links listed below <b>(If any, even worse - all, links aren't working - please [create an issue](https://github.com/synboxdev/WitcherGuessr/issues) for this repository, and/or contact me directly)</b>:

| File              | Mirror 1 ([MEGA.io](https://mega.io/))                    |
| :---------------- | :--------------------                                     |
| Maps              | https://mega.nz/folder/LdVkjDKY#zcwAuFSRo9qztdCwkY68eA    |
| Fablesphere       | https://mega.nz/folder/fctDGBxQ#KGYBZpVSOm3hxyFKAVMzvg    |
| Gaunters World    | https://mega.nz/folder/vU0lWIJa#4ypfQTWZ3Nf0TgrFJSMuxA    |
| Isle of Mists     | https://mega.nz/folder/PJ1VnaZJ#XUtjwkCcs_2hCIJPe7Xp_Q    |
| Kaer Morhen       | https://mega.nz/folder/HI90FQjB#C_s13VewCIT-SNW1V9idHw    |
| Skellige          | https://mega.nz/folder/rUFkhLhb#cBgNa6xib7O6R9dq3C3aEQ    |
| Toussaint         | https://mega.nz/folder/LY9CTIjI#oVVPJ8QOJsFjRZuRXg_BFQ    |
| White Orchard     | https://mega.nz/folder/PdsRXIDS#G_ym63vk9Vd7d_a_c73foQ    |
| Velen & Novigrad  | https://mega.nz/folder/DQUmVAwa#J12l4r6c5Kx8M73IwR_Baw    |

| File              | Mirror 2 ([Filen.io](https://filen.io))                                                           |
| :---------------- | :--------------------                                                                             |
| Maps              | https://drive.filen.io/f/848c07f2-90dc-4ed8-bba5-a6dcdcde5b4a#FGuVUjyERM9HFpCnvFxY31yeMrr5Nnnt    |
| Fablesphere       | https://drive.filen.io/f/60cdb504-b0eb-4fb6-989e-e2efa98eeb02#4Mw3cm4HMdPRIqF5DUoRDEyDFkeGXRck    |
| Gaunters World    | https://drive.filen.io/f/7d5dc03d-9789-4cf3-80e4-2b1bc2fb325f#IBRCFo6nRr4lyJKptBFvheY0ovZGTioy    |
| Isle of Mists     | https://drive.filen.io/f/59f1733a-2041-44b1-91d1-f60a039db8d7#WBfnpJ0Ai0SAq6bCKVTZ3FuniUlkEiwR    |
| Kaer Morhen       | https://drive.filen.io/f/71d25c49-63a2-41f3-aeef-b5ace1978818#kwKEjhMhDa5vnBKxzcdyPa3g7YRPDvXA    |
| Skellige          | https://drive.filen.io/f/74dbb462-1294-4c92-8970-4fa91686a5a5#tI0okcA7ST1DZDz2lm8P5r0ICv2D10l6    |
| Toussaint         | https://drive.filen.io/f/c710bb7c-b304-4280-9785-407783b22cc2#UXV1BfFKBgGJKdbPtcH2pMMZmdOsIWIk    |
| White Orchard     | https://drive.filen.io/f/8bc5a127-93e6-4495-b844-c5a672aa81fe#UkqYl4XusAzoBLRANA2K3DOYcINnKLQi    |
| Velen & Novigrad  | https://drive.filen.io/f/c738589d-dbc4-4bcd-9251-e904de7962aa#RKaxvvEtJp9u0dIcpXw3rV8q9IGhlaVS    |

| File              | Mirror 3 ([Blomp.com](https://www.blomp.com))              |
| :---------------- | :--------------------                                      |
| Maps              | https://sharedby.blomp.com/iokllG    |
| Fablesphere       | https://sharedby.blomp.com/bGYDYW    |
| Gaunters World    | https://sharedby.blomp.com/ytADoU    |
| Isle of Mists     | https://sharedby.blomp.com/OH83L4    |
| Kaer Morhen       | https://sharedby.blomp.com/fG2oD2    |
| Skellige          | https://sharedby.blomp.com/Zo1XTQ    |
| Toussaint         | https://sharedby.blomp.com/O6Dx3Y    |
| White Orchard     | https://sharedby.blomp.com/TPpAMV    |
| Velen & Novigrad  | https://sharedby.blomp.com/NHYZkn    |

5. Now, move the contents of the 'Maps' folder, into 'Maps' folder, within project's directory. Starting from root directory, which has markdown documentation files, path should be <b>.\WitcherGuessr\WitcherGuessr\Assets\UI\Maps</b>. 

    Inside of your Maps folder, within project's directory should look like this: <br> ![Maps directory](/Documentation%20images/MapsDirectory.png)



6. Next, move the contents of each locations' folders, into the 'Locations' folder, within the project's directory. Starting from root directory, which has markdown documentation files, path should be <b>.\WitcherGuessr\WitcherGuessr\Assets\UI\Locations</b>.

    Inside of your Maps folder, within project's directory should look like this: <br> ![Locations directory](/Documentation%20images/LocationsDirectory.png)

7. Make sure, that '.meta' files are also present alongside the actual files. This is very important for Unity to properly 'recognize' and assign values accordingly, within Unity's Editor. This applies to ALL files - maps, locations, prefabs.

8. Now, <b>in theory</b>, if you didn't lose .meta files along the way, and all of the maps and locations are located within their respective directories, the game should start up and work. But, we can double check most important parts, related to the files we've just added to the project's directories:
    
    ### Maps
    1. Make sure that ALL maps have the following import settings, and are assigned to an appropriate 'Maps' folder within Addressables Groups tab. The only thing that may vary between maps is 'Max Size'. Keep it as is. 
    
        ![Map import settings](/Documentation%20images/MapImportSettings.png)

    2. In hierarchy, open up 'Global' game object, and within the Inspector window, within 'Map Manager' script, double-check if all individual maps have their 'Map prefab' and 'Addressable map sprite' values assigned, as shown below. Only 'All maps' selection shouldn't those values assigned.

         ![Map inspector values](/Documentation%20images/MapInspectorValues.png)

    ### Locations
    1. Make sure that ALL locations have the following import settings, are assigned to an appropriate location within Addressables Groups tab. Also - make sure that each location, of a given map, has an appropriate label assigned. See below.

        ![Location import settings](/Documentation%20images/LocationImportSettings.png)

    2. In hierarchy, open up 'Global' game object, and within the Inspector window, within 'Location Manager' script, double-check if all individual locations have their 'Addressable Panoramic Image Texture' values assigned, as shown below.

        ![Location inspector values](/Documentation%20images/LocationInspectorValues.png)

    3. In case 'Addressable Panoramic Image Texture' values within the inspector are NOT assigned, but you're certain that all import settings are correct, and all files are in their appropriate directories - try closing the inspector window, and executing a Unity Editor-only functionality that I've implemented for this project. Within Unity's Editor, open 'Custom' tab, top-left of the editor, and execute 'Initialize location default values'. 
    
        This function, in theory, should double check the import settings of ALL locations, loop over ALL maps, and assign the appropriate location's texture, to aforementioned 'Addressable Panoramic Image Texture' within the inspector. Otherwise - it should throw some error into the Console window, in case some import settings are incorrect. Sometimes, Unity's Editor bugs out, so try re-opening the Editor, or re-running this function a few times, if its goes through, no errors are shown in the console, but also no values are assigned.

---

<h1 style="text-align:center;"><b>Best of luck!</b></h1>