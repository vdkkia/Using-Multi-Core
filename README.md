# Using Multi-Core to process heavy tasks by using Background Worker (C#)

It is essential to apply multi cores of CPU when you use long tasks in C#, because running such tasks in main body of application causes the process to be executed in same thread as UI thread and practically the user can’t use the application. For avoid these problem, we can use BackgroundWorker in Visual Studio Form based application. Subroutines and functions that are called in BackgroundWorkers’ DoWork event, will run in a thread apart from UI thread, So the user can continue using the application.
Sometimes it is needed to use more BackgroundWorkers in order to speed up executing heavy tasks.
An important point to consider in order to use BackgroundWorkers is that how to form the functions to be executable in the Worker’s body.
Another important point when using multiple Workers, is splitting the functions and assigning them to the Workers that is the most important part of work.
For example, following code is used to handle long lasting functions and heavy tasks (in my case sorting, removing duplicates, … of text Data).
