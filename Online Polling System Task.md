# Online Polling System
## Problem Overview
Build a **simple online polling system** that allows users to:
- Create polls with multiple choice options.
- Vote on polls.
- See real-time results of the polls after voting.
- Optionally, track user participation to ensure each user votes only once per poll without the need for registration.

The application should have the following components:

1. Backend API (ASP.NET Core)
2. Frontend Web Interface (React.js)
3. Mobile Application (.NET MAUI)
4. Database (SQL Server)

## Core Features
1. Poll Creation
   - Users can create a poll by specifying:
     - Poll title: A question (e.g., "What's your favorite programming language?")
     - Options: Multiple choice answers (e.g., "C#", "JavaScript", "Python")
     - Poll duration: Set a start and end date for voting.
   - Polls must be stored in the database, including the poll's title, options, and vote counts.

2. Voting on Polls
   - Anyone can vote on a poll.
   - After voting, the user sees the updated poll results (e.g., vote count per option).
   - Users can vote once per poll (optional).
   
3. Poll Results
   - After voting, users can view the **real-time results** of the poll.
   - Results are presented in a clear, visually appealing way, such as a **bar chart** or **pie chart** (Recharts).

4. Poll Expiry
   - Polls can expire after the end date (if a date range is specified), after which voting is disabled and final results are displayed.
   - Expired polls should still be viewable for historical reference but voting should be locked.

5. Admin Features (Optional)
   - Admin users can manage (edit, delete) polls.
   - Admins can see statistics such as how many votes were cast, active vs. expired polls, etc.

## **Bonus Features**
- **Poll Sharing**: Allow users to share polls.
- **Admin Dashboard**: For admins to manage polls, view stats, and delete expired/irrelevant polls.
- **Poll Comments**: Users can leave comments on polls to discuss the results or poll topics.

## **Evaluation Criteria**
- **Backend**: Proper API design for poll creation, voting, and results. 
- **Frontend**: Clean and responsive UI with easy navigation for voting and viewing results. Proper state management with React hooks.
- **Mobile**: Intuitive mobile app that mimics the web app and handles offline functionality.
- **Database**: Efficient design of the database schema and use of relationships (polls, options, votes).
- **Code Quality**: Clear, readable, maintainable and testable code with proper structure, comments, and documentation.