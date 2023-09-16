# <b>Contributing to 'Witcher Guessr'</b>

First and foremost, I appreciate you being interested enough in this project to even consider contributing to it as a whole. The following contents of the document lay out the basic rules and guidelines about contributing to the project. That being said, by no means these are laws set in stone - follow your best judgment, and feel free to propose changes to this document, or the project itself. Any and all positive contributions are always welcome, including features, issues, documentation, guides, and more.

---

## 📄 <b>Table of contents</b>

* [Code of Conduct](#code-of-conduct)
* [GitHub Terms of Service](#github-terms-of-service)
* [Guidelines of contribution](#guidelines-of-contribution)
    * [Choosing a location for panorama image](#choosing-a-location-for-panorama-image)
    * [Visualizing the existing locations in the project](#visualizing-the-existing-locations-in-the-project)
    * [Requirements for to-be-submitted panorama images](#requirements-for-to-be-submitted-panorama-images)
    * [Code conventions](#code-conventions)
    * [Issues](#issues)
* [How to contribute](#how-to-contribute)

---

## <b>Code of Conduct</b>

This project and everyone participating in it are governed by the [Code of Conduct](CODE_OF_CONDUCT.md). By participating and contributing the the project and its contents, you are expected to uphold this code.

---

## <b>Github Terms of Service</b>

If it wasn't already clear - you should always follow [GitHub's terms of service](https://docs.github.com/en/site-policy/github-terms/github-terms-of-service) not only when contributing to this project, but to any and all repositories on the platform - follow the rules!

---

## <b>Guidelines of contribution</b>

## <b>Choosing a location for panorama image</b>

Vastness and visual fidelity of [The Witcher 3: Wild Hunt](https://www.thewitcher.com) might make it difficult to choose a fitting location for a panorama image. Here's a few rules of thumb that I tend to follow, when choosing said location myself:

* <b>A given location either does not exist in the project OR to-be-submitted panorama of an already existing location (or extremely close by) is higher quality.</b> It should be relatively unique in comparison to the already existing locations in the project. Moving a few steps from an existing location and swapping to a different time of day, or adding weather effects can't be considered as a 'unique' location. Either choose a location that is truly unique in comparison to already existing ones, or clearly state that your submitted panorama image should be a replacement for an already existing location. See section [Visualizing the existing locations in the project](#visualizing-the-existing-locations-in-the-project) below for more information.

* <b>A given location is recognizable.</b> Location of your choice should be possible to recognize because of either one of two reasons:

    * Its directly related to either a quest, or a point of interest, which would imply that people would've potentially stumbled upon when playing through the game.
    * There are recognizable visual cues that could help identifying a given location. A building, flag, statue, you name it - something that would help identify the surroundings and pinpoint a given location on the map.

* <b>A given location has a corresponding place on a map.</b> This is an extremely important point! Some locations in the game, despite being visually stunning, will not be valid to be added to the project, because of a simple reason - they can't be pinpointed on the map itself. For example, places like different 'worlds' that you visit with Avallac'h during quest [Through Time and Space](https://witcher.fandom.com/wiki/Through_Time_and_Space) or cave systems either simply don't 'exist' on a map that player could open and identify, are located 'outside the bounds' of a given map, or merely have a mini-map display, and display you somewhere 'under' the map when you open an actual map menu. Therefor, such locations simply will not be suitable for the project.

---

## <b>Visualizing the existing locations in the project</b>

There's a small functionality that I've developed for the sole purpose of making it easier to visualize where the existing locations for each map are located. This functionality exists and can be invoked only within Unity Engine editor. To utilize it, follow the steps below (assuming you've already installed the Unity Engine itself, and are able to follow along):

1. Open the project via Unity Engine.
2. Start the project via the 'Play' button in the editor.
3. In-game, press 'Play' in the main menu, select 'All maps', and press 'Play'.
4. Once you're in-game, and some panorama image is loaded, press 'Guess the location', and choose a map for which you want to visualize all the existing locations.
5. From the Unity's main menu, press 'Custom' -> 'Spawn all locations of current map'.
6. Blue markers will be placed in the entire current map for you to see!

Small thing to note - if you open another map afterwards, initial 'Spawn all location of current map' selection will de-spawn the location markers on the previous map. It works like a 'toggle', so you'll have to press it once more, to spawn the location markers in the current map.

---

## <b>Requirements for to-be-submitted panorama images</b>

There are a few requirements that should be followed, when creating and submitting panorama images as a contribution to the project:

1. Due to GitHub's file size limitation that you can see in [GitHub's documentation](https://docs.github.com/en/repositories/working-with-files/managing-files/adding-a-file-to-a-repository#adding-a-file-to-a-repository-on-github) and [attaching files documention article](https://docs.github.com/en/get-started/writing-on-github/working-with-advanced-formatting/attaching-files), file size of submitted panorama would have to be below 10MB. If necessary, you could upload the image to a platform like Imgur which has file size limit of 20MB as per [this help article](https://help.imgur.com/hc/en-us/articles/115000083326-What-files-can-I-upload-Is-there-a-size-limit-). This might be a subject to change, so if you have ideas for a suitable alternative of submitting images - feel free to create an issue with a 'Suggestion' label, or start a discussion regarding this topic.
2. When submitting a panorama image - submit AT LEAST two panorama images of a given location, with different times of day, and potentially - different weather, so that best fitting choice could be made.
3. Additionally, add in-game coordinates (which can be retrieved using in-game Debug Console - See more at ['How to Enable the Debug Console in Witcher 3'](https://commands.gg/witcher3/blog/enable-console)) or at least submit a zoomed in image of map of a given location.
4. Panorama image (location in-game), should not have any spawned in objects, entities or NPCs that are NOT present in the un-modded game. Things like alternate outfits for NPC characters, or monsters that are naturally spawn in a given area are fine.
5. Repeating what was described previously - familiarity and recognizability are key factors that should always be considered when submitting a panorama image. See section [Choosing a location for panorama image](#choosing-a-location-for-panorama-image) above.

---

## <b>Code conventions</b>
In order for the project to remain well-built and structured you should align with stylistic of the code in the repo already. Good place to start would be following [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

---

## <b>Issues</b>
I'd consider [GitHub Issues](https://docs.github.com/en/issues/tracking-your-work-with-issues/about-issues) as a standardized and widely accepted way of contributing to the project and its contents. It covers everything from feature requests, bugs, suggestions to questions. If you wish to read further, GitHub documentation on [Creating an issue](https://docs.github.com/en/issues/tracking-your-work-with-issues/creating-an-issue) has got you covered. As a general rule of thumb, here's a few bullet-points I'd like you to follow, when creating an issues, regardless of its type:
* Title of the issues should be clear, concise and descriptive.
* Utilize existing [labels](https://docs.github.com/en/issues/using-labels-and-milestones-to-track-work/managing-labels) for your issues to be more informative. If you believe an additional label should be added - create an issue with a default label 'enhancement', where you'd explain what that new label should be, as well as provide reasoning as to why it should be added to the project.
* Provide an informative, comprehensive description of the bug, suggested feature or an enhancement that you believe should be changed/added or improved in the project.
* Feel free to provide pictures, flowcharts or any other forms of visual representation of the suggestion or a problem. Make it easy for others to understand you, and get on the same page.

---

## <b>How to contribute</b>

1. Clone the repository and make a new branch. 'Getting started' section in [README.md](README.md) document might be a good starting point.
2. Make any and all changes to your branch. Whatever that might be - a bugfix, a new functionality or addition of new locations.
3. Do NOT clump together multiple things into the same branch - a single branch should cover a single thing (I.e. A single bugfix, functionality or addition of added locations).
4. Make sure the project builds successfully and doesn't throw errors.
5. [Create a pull request](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request) with understandable, short and concise description of changes.