# Escape the Clocktower

## Group Members

- Danna Bundogji
- Anwar Bakhos

## Game Description

- First person 3D escape room game made in Unity (URP)
- You play as Arthur Vance, a journalist who broke into the St. Jude Municipal Clocktower after getting an anonymous tip that Julian Cross, the man who built the clock in 1996, never actually left the tower
- You get knocked out and wake up locked in the study at 11:45 PM
- At midnight the "Midnight Protocol" goes off and seals the tower, so you have 15 minutes to get out
- Notes and Cross's journal around the tower tell the story of what happened to him
- Levels:
  - **The Study**: starting room with a grandfather clock puzzle
  - **The Clockwork Shaft**: a tall tower shaft with spiral stairs, a gear machine puzzle and a swinging pendulum
  - **The Top of the Tower**: the belfry hatch where you ring the bell to escape

## Current Project Target

- Playable build from the main menu all the way to the ending
- Two working puzzles (clock puzzle and gear train puzzle)
- Moving obstacle (pendulum that knocks you back)
- 15 minute timer with win and lose screens
- Main menu, instructions, pause menu, win screen and lose screen
- Directional light plus point and spot lights inside the rooms
- Door, lever, gear and clock hand animations

## Final Project Goals

- Build out the belfry at the top (bell, hatch and rope) as the final room
- More puzzles and more story notes
- Music and more sound effects
- More decoration and polish in every room
- Gameplay video

## Group Responsibilities

### Danna Bundogji (environment, presentation and documentation)

- Room design and decoration
- Lighting
- Materials and colors
- Main menu, instructions screen, pause menu and win screen
- Audio
- README and testing

### Anwar Bakhos (gameplay and code)

- Player movement and camera
- Key and object interaction
- Door opening logic
- Puzzle code
- Moving obstacle (pendulum)
- Main gameplay scene

Both of us test the game and commit to the repo.

## How to Play

### Controls

- **WASD** - move
- **Mouse** - look around
- **Shift** - sprint
- **Space** - jump
- **E** - interact (read notes, pick up, open, place gears, pull lever)
- **Q** - turn the clock hands back (when looking at the grandfather clock)
- **G** - drop the gear you are holding
- **F** - flashlight
- **Esc** - pause

### Goal

- Get out of the tower before the clock hits 12:00 AM
- Read the notes, they have the hints you need
- If the timer runs out you lose
- Getting hit by the pendulum costs 20 seconds and sends you back to the last checkpoint

### Running it

- Open the project in Unity **6000.5.9f1**
- Open `Assets/Scenes/MainMenu` and press Play

## Secret Solutions (spoilers!)

<details>
<summary>Click to show the full solution</summary>

### The Study

- Read Cross's journal on the desk, he says the clock case opens at the hour he finally leaves
- Read the train ticket on the desk, it says it departs at **1:45 AM**
- Go to the grandfather clock and set it to **1:45** (E moves it forward 15 min, Q moves it back)
- The clock case opens and you get the **Study Key**
- Use the key on the study door and go up the stairwell

### The Clockwork Shaft

- Read the maintenance log on the gear machine: the great wheel has to turn once for every 3 turns of the drive, and the middle gear doesn't matter
- There are 5 gears around the shaft:
  - 18 tooth gear - on the floor to the left when you walk in
  - 14 tooth gear - on the floor to the right of the machine
  - 10 tooth gear - on the bottom part of the stairs
  - 6 tooth gear - about 2/3 of the way up the first loop of stairs
  - 20 tooth gear - near the top of the first loop
- Put the **6 tooth** gear on the **left** peg (drive)
- Put **any** gear on the **middle** peg
- Put the **18 tooth** gear on the **right** peg (great wheel)
- 18 / 6 = 3 so the ratio works
- Pull the lever, the tower starts up and the belfry hatch opens

### The Top of the Tower

- Once the clock is running the pendulum starts swinging across the second loop of stairs
- Wait for it to swing away before you walk past
- Keep climbing to the very top and ring the bell to escape
- If you go up before fixing the gears the hatch won't open

</details>

## Gameplay Video

https://drive.google.com/file/d/17qvkBjwoiEk8J0TfuWNedIILljGULxzO/view?usp=sharing
