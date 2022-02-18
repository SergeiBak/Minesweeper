# Minesweeper
<img width="276.48" height="155.52" src="https://github.com/SergeiBak/PersonalWebsite/blob/master/images/Minesweeper.png?raw=true">

## Table of Contents
* [Overview](#Overview)
* [Test The Project!](#test-the-project)
* [Code](#Code)
* [Technologies](#Technologies)
* [Resources](#Resources)
* [Donations](#Donations)

## Overview
This project is a recreation of the classic puzzle game known as Minesweeper, with its origins rooted in the 1960s. This solo project was developed in Unity using C# as part of my minigames series where I utilize various resources to remake simple games in order to further my learning as well as to have fun!   

Minesweeper consists of a grid with a number of hidden mines scattered randomly throughout. You can reveal covered cells with a left mouse click, or right click them to flag them if you believe a mine lies underneath. Number cells display the amount of mines immediately surrounding said cell (If you placed flags on where you believe mines are located around the cell, you can click the mouse scroll wheel onto the number cell to automatically flip all the non-flagged cells around it). Be careful not to reveal a mine or you lose! To win the game, you must reveal all non-mine cells on the board.

## Test The Project!
In order to play this version of Minesweeper, follow the [link](https://sergeibak.github.io/PersonalWebsite/pong.html) to a in-browser WebGL build (No download required!).

## Code
A brief description of all of the classes is as follows:
- The `Board` class handles the rendering of the board based on mine locations & cell states.
- The `Cell` class represents the struct for each cell on the board
- The `GameManager` class contains all of the base state logic for the running of the game including win/lose conditions
- The `MenuManager` class handles all of the menu UI interaction & scene loading

## Technologies
- Unity
- Visual Studio
- GitHub
- GitHub Desktop

## Resources
- Credit goes to [Zigurous](https://www.youtube.com/channel/UCyaKsKqYTghxgAqywfefAzg) for the helpful base game tutorial!
- I made use of [Unity PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) for saving stats
- Minesweeper algorithms overview [reference](https://dash.harvard.edu/bitstream/handle/1/14398552/BECERRA-SENIORTHESIS-2015.pdf)
- Gameplay [reference](https://minesweeperonline.com/)

## Donations
This game, like many of the others I have worked on, is completely free and made for fun and learning! If you would like to support what I do, you can donate at my metamask wallet address: ```0x32d04487a141277Bb100F4b6AdAfbFED38810F40```. Thank you very much!
