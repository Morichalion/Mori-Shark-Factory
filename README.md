# Mori-Shark-Factory
A simple tool that abstracts the command-line arguments for nod-ai/SHARK
Works 

It's written in Godot 4 beta 15. Mono version. 

I only tested this on Windows. 

Currently it only works with the regular install, the .exe will not work. 



#Setup and Usage: 
1. Install SHARK using the command line instructions found here: https://github.com/nod-ai/SHARK
2. Extract the Mori-Shark-Factory.zip file to some directory somewhere.
3. Run the app. 
4. Pull the "Prompt" dropdown optiont to "Shark Install" [Like this]( https://github.com/Morichalion/Mori-Shark-Factory/blob/main/Shark-Location.png)
6. Enter the path. 
7. Set the dropdown back to prompt. [Like this](https://github.com/Morichalion/Mori-Shark-Factory/blob/main/promps.png)
8. Enter your prompts
9. On the first run, SHARK will download the .vmbf files, so it'll take a bit.
10. Another quirk: the first time you generate a file the cmd window may not close on it's own. Close it and restart.
11. Have fun. 


The output is set write to a directory on desktop (~\Desktop\Mori-Shark-Factory\sharkOutputs). The shark install location is saved in appdata. 

If anyone's good with c# and wants to review the code lemme know. 
