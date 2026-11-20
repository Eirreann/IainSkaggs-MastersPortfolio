# MSc Portfolio: Serious Games and Virtual Reality

This repository contains the Unity project files for four of the games I made for my MSc in Serious Games and Virtual Reality at the Glasgow School of Art's School of Simulation and Visualisation, which I completed with Distinction in 2019. Each game is built around a learning or empathy goal, with topics ranging from Scottish heritage and Gaelic grammar to gender discrimination and emigration.

The repo holds project files only. Playthrough videos of every game, along with the design documents and my dissertation, are in the [MSc portfolio on my website](https://iain-skaggs.com/msc-portfolio). The website also covers three MSc projects that aren't in this repo: *Tele-Former*, *The Freeform Fable*, and the board game *Flying Colours*.

## Contents

| Folder | Game | Module | Unity version | Team | My role |
|---|---|---|---|---|---|
| `S1_CaledoniaAdrift` | *Caledonia Adrift* | Interactive Heritage Visualisation | 2018.2.16f1 | Two, plus a composer | Designer and developer |
| `S2_Mono` | *Mono* | Serious Games Development | 2018.3.8f1 (made in 2018.2) | Three | Sole developer, level designer, and sound designer |
| `S2_SoundOfMemories` | *Sound of Memories* | Audio for Games | 2018.3.9f1 | Three | Co-designer and sole Unity developer |
| `S3_GramarGuLeor` | *Gràmar gu Leòr!* | MSc dissertation | 2018.3.8f1 | Solo | Researcher, designer, and developer |

The S1 to S3 prefixes mark the semester each game was made in. `S1_CaledoniaAdrift` holds two Unity projects, one for each language edition.

---

## *Caledonia Adrift*

A third-person platformer about Scotland's ancient past, where real Pictish carved stones are your checkpoints. Neolithic artefacts are scattered along the way to collect, and each stone and artefact unlocks an entry in the pause menu's Discoveries tab. We built the game in English, and I later made a Gàidhlig (Scottish Gaelic) edition for a mid-year degree show, translated with support from a colleague at MG ALBA. The Gàidhlig edition has no public build, but its full project is here. The [*Caledonia Adrift* page on my website](https://iain-skaggs.com/caledonia-adrift-3rd-person-platformer) has a full playthrough and my process journal.

**In the code** (`Assets/Scripts`, identical in both editions, so the translation lives entirely in the scenes and UI text):
- `PlayerRespawnTracker`: checkpoints tied to the last carved stone the player reached, with a game-over screen that returns them there. Menus set their selected button so the whole game can be played with a gamepad.
- `UITriggerController`: trigger-driven tutorial pop-ups that hide while a menu is open.
- `PickupBehaviour` and `InfoText`: the collectable stones and artefacts, and their Discoveries entries.
- `KeepPlayerOnPlatforms`: moving platforms that carry the player.

**Team:**
- Antony Lingard: 3D models and learning content (MSc Heritage Visualisation)
- Fiona McNeill: music and sound effects

## *Mono*

A 2.5D puzzle platformer about gender discrimination, set in a dystopian city where everyone's role in society is determined by the shape of their head. The player is a Square who starts to question their place, and learns to disguise themselves by swapping their head for role objects found around the city, to get past role scanners and change how other citizens treat them. The [*Mono* page on my website](https://iain-skaggs.com/mono-25d-puzzle-platformer) has a full playthrough and our design document.

**In the code** (`Assets/Scripts`):
- `Player/RoleSwapper`: lets the player hold one role while wearing another, and returns dropped pickups to where they were found.
- `RoleScanner` and `DefunctRoleScanner`: role-gated scanners with light, colour, and audio feedback, including a deliberately faulty one.
- `TriggerFungusFlowchart`: NPC dialogue in Fungus that changes with the role the player is wearing.
- `Player/Player2DController`, `LadderClimb`, and `PushObj`: the 2D platformer controller, ladders, and objects to push and pull.
- `IntroZone*`: the opening scene, which loops the player character's daily commute until the player breaks role norms by sitting in the wrong section of a subway.

The playable levels are in `Assets/Scenes/Main`, in build order. Greybox, environment, and test scenes sit alongside them.

**Team:**
- Ibari Abani: animation, environment design, and 3D models (MSc Serious Games and Virtual Reality)
- Sharon Ferris: UI and art design (MSc Serious Games and Virtual Reality)

## *Sound of Memories*

A first-person exploration game that tells its story through sound. The player explores a single room to uncover the story of a young man who emigrated to America, told through his letters home to his mother. A pair of "Time Goggles" switches the room between the present and three remembered versions of it, from the turn of the century to the Great Depression, each with its own soundscape. The [*Sound of Memories* page on my website](https://iain-skaggs.com/sound-of-memories-audio-experience) covers the game in more detail.

**In the code** (`Assets/Scripts`):
- `ViewChange`: the time shift, whose sound effect plays forwards as the player enters a memory and in reverse as they leave it, in sync with a screen fade.
- `Interaction`: one raycast interaction system covering pickups, lights, the goggles, and sounds that play only while the player is looking at an object.
- `RecordPlayerTrigger`: a record player that plays whichever record is placed on it, each record routed through its own mixer channel.
- `FootstepMatTrigger`: footsteps that change with the floor surface.

I implemented every sound my teammates created, using separate mixers for background, foreground, and interface audio, so the team could play through the whole game and balance it in one place.

**Team:**
- Anthony Denton: co-design and sound (MDes, Sound for the Moving Image)
- Joe Nowicki: sound (MDes, Sound for the Moving Image)

## *Gràmar gu Leòr!* (MSc dissertation)

A 2D puzzle game that teaches adult learners to conjugate Scottish Gaelic regular verbs. Each of its 24 maze puzzles gives the player an English sentence to translate into Gaelic: they draw a line from a verb tense, through symbols for verb forms and past obstacles that can strip the tense or form away, to build the Gaelic verb and hear it spoken. I based the design on a literature review and a survey of adult Gaelic learners, then evaluated it in playtests. The [project page on my website](https://iain-skaggs.com/gramar-gu-leor-puzzle-game) covers the findings and links to the full dissertation.

**In the code** (`Assets/Scripts/Core Puzzle Mechanics`):
- `DrawPuzzleLine`, `FollowMouse`, and `IntersectionBehaviour`: drawing the line through the maze's intersections, and undrawing it by retracing.
- `ConditionBehaviour` and `ObstacleBehaviour`: the tense and form pickups and the obstacles, which detect the side the line entered from so that retracing undoes their effect.
- `PuzzleManager`: tracks the current tense and form, and drives the Gaelic text and audio output.
- `FinishLine` and `LevelManager`: checking the player's answer, and moving between puzzles.

`Assets/Scripts/_Old` keeps three earlier line-drawing prototypes, and `Assets/Scenes/Testing/PuzzleDesign_Greybox` holds the puzzle layouts.

**Credits:**
- MG ALBA: Gaelic audio
- Creative Commons clipart: backgrounds and props

---

## Opening the projects

- Each project opens in Unity Hub: there are five in total, counting both editions of *Caledonia Adrift*. Use the editor version in the table above, available from [Unity's download archive](https://unity.com/releases/editor/archive).
- The `Library` folders aren't committed, so the first open of each project reimports all of its assets, which takes a while.
- The repo is about 1.8 GB, mostly art and audio.
- Third-party assets and plugins, such as Fungus, ProBuilder, and several Asset Store art packs, are included so that the projects open complete. They belong to their creators, and each game's credits list them.

## A note on the code

These projects date from 2018 and 2019, and the code reflects where I was as a developer then. Almost every line is commented, with the assessors in mind, and state is often tracked with flags and strings where I would now use enums, events, and interfaces. For more recent examples of my work, see my game jam repos: [GMTK Game Jam 2024](https://github.com/Eirreann/GMTK_GameJam_2024), [2025](https://github.com/Eirreann/GMTK_GameJam_2025), and [2026](https://github.com/Eirreann/GMTK_GameJam_2026).