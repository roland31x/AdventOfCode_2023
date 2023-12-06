# AdventOfCode_2023

My solutions for Advent of Code 2023 in C# .NET 7.0

This time i'm going for leaderboard position so I wrote a little code to access their api for inputs and submissions found in AOCApi.dll

You can use it as well, it only has two functions, one to fetch the input and save it on your pc:

    AOCApi.InputGetter.GetInput(
        string InputPath,           // needed to check if input is already present if not where to save it
        int Year,                   // which year to fetch from
        int Day,                    // which day to fetch
        string SessionCookieValue   // you need to set this manually because inputs are account tied
        );  

        // returns List<string> that contains every line for your input text, and saves it onto the inputpath.

and the submission function which automatically sends in your request for a part and will print you wether your answer was good or not:

    AOCApi.OutputSubmitter.Submit(
            string answer,              // the answer you got from your code
            int Year,                   // which year to fetch from
            int Day,                    // which day to fetch
            int Part,                   // 1 or 2 is needed since the website has to know which part it needs to check your answer against
            string SessionCookieValue   // same as input, you need this cookie value to submit on your accounts behalf
            ); 
            
        // returns string that contains the response from their website about your result.
