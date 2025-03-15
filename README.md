# Smelter Panic
A Job Interview Task made to the following specifications: https://docs.google.com/document/d/1rrw-6YkY0lDDSkZYfLuIj52B4oEelROwJbpBYjK_WJ8/edit?usp=sharing
# Gameplay
1. Enter the Playmode.
2. Click on Machines listed in the right-side panel to open Machine View.
3. Drag items from your inventory (left-side panel) onto the Recipes listed in Machine View.
4. Once all the required items are dragged onto Recipe, you can start that recipe via the Craft button.
5. The Timer will start, and the Machine will be unavailable for the crafting duration.
6. If crafting is successful, the crafted item will be added to your inventory.
7. To restart the game, leave the Playmode & enter again.
# Design Choices
1. Single Machine can craft only one Recipe at a time. Multiple Machines can craft their Recipes at the same time. That was my interpretation of design guideline #2. This can be easily adjusted to allow multiple simultaneous Recipe runs per Marchine if need be.
2. Quests can provide only a single Reward. This can be easily adjusted to allow multiple rewards per Quest. Some quests can be invisible to the Player - bonus-providing item Reward for completing all the Quests is handled via such invisible quest.
3. Machines View lists all the Recipes valid for the given Machine. I wasn't sure whether those should be visible to the Player but ultimately decided for it. This can be easily adjusted to obfuscate Recipes from the Player.
# Project Structure
1. _Data contains all the ScriptableObject assets. I tried to make the game logic as data-dependent as possible.
