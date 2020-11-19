# OrbitalWitness technical test

## Explanation

I have time myself to exactly 3 hours of work this is an unfinished project for review, the stage is a Proof of Concept which is partially working.
I pondered a few approaches to implement a solution for the problem at hand, one approach involved a rather complex regex that would have done the 
job efficiently with very few issues including the edge case handling.
The other approach was breaking down the text and writing an algorithm to extract the correct parts of the text into the object.

I've chosen a middle path which has the disatvantage of not being efficient nor aesthetically pleasing, 
I believe this test was not to verify my regex knowledge but rather to verify my thought process and algorithm skills.

In the current state the project is not clean, it would require refactoring and optimization, however as the 3 hours have elapsed I've decided to not perform the refactoring.
Not doing any refactoring will also help show every step of my journey to find the solution. I will add comments within code with parts which should be extracted and optimisations to be done.

## Steps:

First 30-40 minutes were spent reading the spec and the data set and attempting to come up with a solution on paper for the issue at hand. As I could not find a simple solution I decided to proceed by 
tackling one issue at a time, firstly I created the skeleton of the project with the DTO models in which I would read the data. After that I moved on to reading from the input sample file hardcoded 
the path and started working on the processor.

At this point I spent one hour on preparation and it was time to start working on the actual implementation, 
I started off by writing the test for the parser, the test was there for me to debug various parts
of the implementation.

After that I spent the next 20-30 minutes working on a regex, after that I realized that the whole project was 
solved by that regex which would smartly lookahead/behind and isolate all columns, so I scrapped & adjusted it so that I can actually write a C# solution to
the problem. I decided to extract from the first line of every entry the positions of each column and use those as delimiters, a simpler regex did that for me so I allowed myself to cheat a bit. 
After that it was only a matter of making adjustments until the test passed as expected

After the test passed I ran the sample file which failed, for the final 30 minutes I worked on edge cases found added one extra test and a few more validations.

## Features:
[x] Interpret records (majority)
[x] Interpret records with notes (many)
[x] Interpret records with null notes (edge case)
[x] Interpret invalid records (closed cases)
[] Interpret records with missing columns (edge case?)
Anything else which I may have missed as I didn not implement appropriate logging


This project requires the following:
- Edge case unit test scenarios
- Optimisation & Refactoring
- Logging of failures and unexpected scenarios