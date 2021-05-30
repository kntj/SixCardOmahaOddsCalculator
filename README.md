# SixCardOmahaOddsCalculator
6 Card Omaha Odds Calculator

## 1. enable library
see SixCardOmahaOddsCalculator\lib\PokerHandEvaluator\cs\readme.md  
(include install .net)

## 2. publish
```
SixCardOmahaOddsCalculator\src> dotnet publish -c release -o publishfolder\
```

## 3. execute
input console
```
publishfolder> o6calc
Community Card:KdTd3c
Except Card:5c
Player 1 Card:KhKc7sJs6c9s
Player 2 Card:QdJdAh9h2c2s
Player 3 Card:
combinations:630
board[Kd Td 3c]
except[5c]
[Kh Kc 7s Js 6c 9s] ( 51.27%)
[Qd Jd Ah 9h 2c 2s] ( 48.73%)
```  
input text file  
```
publishfolder> o6calc SixCardOmahaOddsCalculator\samplefile\flop.txt
```  
option suit color  
```
publishfolder> o6calc -c
```
