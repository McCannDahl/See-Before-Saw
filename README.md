
# Pariveda-Hackathon-2022

## Getting started (only for Windows 10)
1. To get started, open the src/SeeBeforeSaw folder in unity and press the play button
![image](https://user-images.githubusercontent.com/19883817/154108888-3281f04d-f2a6-4f77-bb9b-68f5adcbabc1.png)
2. (optional) to run the app on the hololens or the emulator, follow steps 8-10 below

## Creating a Hololens Project in Unity
Follow this tutorial https://docs.microsoft.com/en-us/learn/modules/learn-mrtk-tutorials/1-3-exercise-configure-unity-for-windows-mixed-reality but don't use the recommended settings at the beginning of the tutorial. Instead, use these recommended settings https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/choosing-unity-version using openXR.  
Roughly the steps in the tutorial are
1. Install Unity Hub
2. Install Unity 2020.3 LTS through unity hub (add Universal Windows Platform Build Support and Windows Build Support (IL2CPP))
3. Create a new 3d project
4. Set the build settings specified in the tutorial
5. Use the mixed reality feature tool to add features to the unity project
6. Configure the project settings
7. Create a scene, add a cube, click play
8. Build the unity project
9. Open it with VS
10. Run the solution either on the actual hololens or an emulator (https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-the-hololens-emulator)
![image](https://user-images.githubusercontent.com/19883817/154140984-8f109550-9811-4c16-8c86-798fbc4e53e0.png)

## Ideas
The app starts with a table with building models on it, with an empty circle in the middle. Physics is enabled.  
![Slide6](https://user-images.githubusercontent.com/19883817/154164888-8944f105-cce9-4fd4-96ea-b4b7ab65261c.PNG)  
If the user puts a model in the circle, then the user is immersed into a 3d rendering of the building's frame. The user has presented details about the project and material lists.
![Slide7](https://user-images.githubusercontent.com/19883817/154164896-3670f3c9-a760-457d-9244-241e223f2e7b.PNG)
Additional ideas include:  
- edit/add project details
- edit/add material list details like quantity or dimensions
- project/material list timeline
- add beams to 3d space
- see stresses calculations (I.e. this is why you can't remove that wall)

## Current State
2/16/2022
![image](https://user-images.githubusercontent.com/19883817/154416873-54333382-48f6-4dec-ac14-a546a5048773.png)
2/17/2022
![image](https://user-images.githubusercontent.com/19883817/154619432-d0d5e683-a464-4838-a8ca-adf94929d2f9.png)
3/8/2022
![image](https://user-images.githubusercontent.com/19883817/157327722-29a73e55-8547-4bb3-858a-80d919432b03.png)

## How to use hands in Unity
Show left hand - hold shift  
Show right hand - hold space bar  
Rotate left hand - hold shift and left control  
Keep left hand where it is - press t while hand is in desired place (press t again to make hand go away)  
Bring up hand menu - Rotate the left hand and press t to keep it in place, then press space bar and move the right hand to the desired position  

## State Management
We use a singleton subscriber pattern (delegates and events) to trigger events and manage changes in state (like setting permissions/switching roles).
The "StateManager" object has the "Event Publisher" script attached to it and contains the code for all these events.  
![image](https://user-images.githubusercontent.com/19883817/157328890-ba2c9dd2-4ee9-4bb2-bc2f-c9252373070b.png)  
If you want to create a new event, in the EventPublisher script:
1. Add the event to the "EventPublisherEvents" (ex: "TruckButtonPressed")
2. Create an event delegate (ex: "public delegate void TruckButtonPressedHandler();")
3. Create an event handler (ex: "public event TruckButtonPressedHandler TruckButtonPressed;")
4. Create an event function (ex: "public void CallTruckButtonPressed() => TruckButtonPressed?.Invoke();")
5. In the "AddListener" function, map the event to the enum so that it can be used by the helpful scripts (see helpful scripts section below) (ex: "case EventPublisherEvents.TruckButtonPressed:")
Now this event can be called, and listened to anywhere in the application!
For example, you could create a button that calls "CallTruckButtonPressed" and you could have a game object that listens to the "TruckButtonPressed" event. We have lots of examples of this throughout the app. Let me know if you need more details.

## How to navigate Scene Heiarchy
![image](https://user-images.githubusercontent.com/19883817/157329690-c8a9219d-e590-4f72-8341-0c6248a72287.png)  
Most of this heiarchy comes with MRTK.
1. The State Manager is the one we discussed in the section above. It handles all events in the app.
2. The PermissionsRequired GameObject is the one that is shown after someone clicks on a role
3. The Presentation GameObject is made up of the inital flow of the app. Generally a user wouldn't ever return to this "page", but during our presentation we will switch roles to showcase all the different aspects of the app
4. The 3d text generator is an asset I found online that creates 3d text. It doesn't represent anything in 3d space, it just builds 3d text game objects.
5. The "hand or near menu" is the menu that shows up when you flip over your wrist
6. Everything else listed here is part of MRTK

## Helpful Scripts
1. EventPublisher - create and subscribe to events
2. HideOrShowOnEvent - hide or show on a particular event
3. HideOrShowOnPermission - only show something if a cirtain role is selected
4. HideOrShowOnProjectOpen - only show if the given project is open
5. KeepSameRelativePosition - keep a relative posiiton to another game object
6. KeepSameRelativeRotation - keep a relative rotation to another game object
7. ShowerAndHider - use this to hide or show elements. It is faster than "GameObject.setActive". This script goes through and hides/shows the render of each game object. This is helpful for when showing objects with lots of children like the building frame.

## Task Flow
![See Before Saw 2 drawio](https://user-images.githubusercontent.com/19883817/159144388-55eb61b9-e042-4f3c-9eab-382c8d7783a8.png)
