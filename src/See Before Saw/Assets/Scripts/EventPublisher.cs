using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PermissionLevel
{
    NONE = 0,
    BUILDER = 1,
    ARCHITECT = 2,
    DEALER_OR_DISTRIBUTOR = 3,
    MILLS = 5,
}
public enum EventPublisherEvents
{
    NONE = 0,
    ParivedaLogoPressed = 1,
    ChangePermissions = 2,
    SwitchRolesButtonPressed = 3,
    HouseGrabbed = 4,
    OpenProjectButtonPressed = 5,
    ViewProjectsButtonPressed = 6,
    HandMenuOpened = 7,
    DesignButtonPressed = 8,
    ExitDesignButtonPressed = 9,
    DesignMenuSliderUpdated = 10,
    BuildingFrameUpdated = 11,
    FinalizeDesignButtonPressed = 12,
    ManageInventoryButtonPressed = 13,
    ExitInventoryButtonPressed = 14,
    ExitOptimizationButtonPressed = 15,
    OptimizationSliderUpdated = 16,
    CutWoodButtonPressed = 17,
    StartOptimization = 18,
    CutWoodFinished = 19,
}


public class EventPublisher : MonoBehaviour
{

    public PermissionLevel permissionLevel = PermissionLevel.NONE;
    public UnityEvent OnAppStartCallEvents; // use this to trigger events on app start for testing
    public bool UseOnAppStartCallEvents = false;
    public GameObject projectCurrentlyBeingDesigned;

    //Delegate definition
    public delegate void BuildingFrameUpdatedEventHandler(GameObject woodBeam, bool willBeDestroyed = false);
    public delegate void ChangePermissionsEventHandler(PermissionLevel n);
    public delegate void ParivedaLogoPressedEventHandler();
    public delegate void SwitchRolesButtonPressedHandler();
    public delegate void HouseGrabbedHandler(GameObject project);
    public delegate void OpenProjectButtonPressedHandler();
    public delegate void ViewProjectsButtonPressedHandler();
    public delegate void HandMenuOpenedHandler();
    public delegate void CloseProjectButtonPressedHandler(GameObject project);
    public delegate void DesignButtonPressedHandler(GameObject project);
    public delegate void ExitDesignButtonPressedHandler();
    public delegate void FinalizeDesignButtonPressedHandler();
    public delegate void DesignMenuSliderUpdatedHandler(SliderEventData data);
    public delegate void ManageInventoryButtonPressedHandler();
    public delegate void ExitInventoryButtonPressedHandler();
    public delegate void ExitOptimizationButtonPressedHandler();
    public delegate void OptimizationSliderUpdatedHandler(SliderEventData data);
    public delegate void CutWoodButtonPressedHandler();
    public delegate void StartOptimizationHandler();
    public delegate void CutWoodFinishedHandler();


    //Event Definition
    public event BuildingFrameUpdatedEventHandler BuildingFrameUpdated;
    public event ChangePermissionsEventHandler ChangePermissions;
    public event ParivedaLogoPressedEventHandler ParivedaLogoPressed;
    public event SwitchRolesButtonPressedHandler SwitchRolesButtonPressed;
    public event HouseGrabbedHandler HouseGrabbed;
    public event OpenProjectButtonPressedHandler OpenProjectButtonPressed;
    public event ViewProjectsButtonPressedHandler ViewProjectsButtonPressed;
    public event HandMenuOpenedHandler HandMenuOpened;
    public event CloseProjectButtonPressedHandler CloseProjectButtonPressed;
    public event DesignButtonPressedHandler DesignButtonPressed;
    public event ExitDesignButtonPressedHandler ExitDesignButtonPressed;
    public event FinalizeDesignButtonPressedHandler FinalizeDesignButtonPressed;
    public event DesignMenuSliderUpdatedHandler DesignMenuSliderUpdated;
    public event ManageInventoryButtonPressedHandler ManageInventoryButtonPressed;
    public event ExitInventoryButtonPressedHandler ExitInventoryButtonPressed;
    public event ExitOptimizationButtonPressedHandler ExitOptimizationButtonPressed;
    public event OptimizationSliderUpdatedHandler OptimizationSliderUpdated;
    public event CutWoodButtonPressedHandler CutWoodButtonPressed;
    public event StartOptimizationHandler StartOptimization;
    public event CutWoodFinishedHandler CutWoodFinished;


    public void CallBuildingFrameUpdated(GameObject woodBeam, bool willBeDestroyed = false) => BuildingFrameUpdated?.Invoke(woodBeam, willBeDestroyed);
    public void CallChangePermissions(PermissionLevel n) {
        permissionLevel = n;
        ChangePermissions?.Invoke(n);
    }
    public void CallParivedaLogoPressed() => ParivedaLogoPressed?.Invoke();
    public void CallSwitchRolesButtonPressed() => SwitchRolesButtonPressed?.Invoke();
    public void CallHouseGrabbed(GameObject project) => HouseGrabbed?.Invoke(project);
    public void CallOpenProjectButtonPressed() => OpenProjectButtonPressed?.Invoke();
    public void CallViewProjectsButtonPressed() => ViewProjectsButtonPressed?.Invoke();
    public void CallHandMenuOpened() => HandMenuOpened?.Invoke();
    public void CallCloseProjectButtonPressed(GameObject project) => CloseProjectButtonPressed?.Invoke(project);
    public void CallDesignButtonPressed(GameObject project)
    {
        projectCurrentlyBeingDesigned = project;
        DesignButtonPressed?.Invoke(project);
    }
    public void CallExitDesignButtonPressed()
    {
        ExitDesignButtonPressed?.Invoke();
        projectCurrentlyBeingDesigned = null;
    }
    public void CallFinalizeDesignButtonPressed()
    {
        FinalizeDesignButtonPressed?.Invoke();
        CallExitDesignButtonPressed();
    }
    public void CallDesignMenuSliderUpdated(SliderEventData data) => DesignMenuSliderUpdated?.Invoke(data);
    public void CallManageInventoryButtonPressed()
    {
        ManageInventoryButtonPressed?.Invoke();
    }
    public void CallExitInventoryButtonPressed() => ExitInventoryButtonPressed?.Invoke();
    public void CallExitOptimizationButtonPressed() => ExitOptimizationButtonPressed?.Invoke();
    public void CallOptimizationSliderUpdated(SliderEventData data) => OptimizationSliderUpdated?.Invoke(data);
    public void CallCutWoodButtonPressed() => CutWoodButtonPressed?.Invoke();
    public void CallStartOptimization() => StartOptimization?.Invoke();
    public void CallCutWoodFinished() => CutWoodFinished?.Invoke();

    //Singleton
    public static EventPublisher i;
    internal GameObject projectCurrentlyBeingOptimized;

    protected EventPublisher() { i = this; }
    void Start()
    {
        if (UseOnAppStartCallEvents)
        {
            OnAppStartCallEvents?.Invoke();
        }
    }

    public void AddListener(EventPublisherEvents myEvent, Action action)
    {
        switch (myEvent)
        {
            case EventPublisherEvents.ParivedaLogoPressed:
                ParivedaLogoPressed += action.Invoke;
                break;
            case EventPublisherEvents.ChangePermissions:
                ChangePermissions += (_) => action.Invoke();
                break;
            case EventPublisherEvents.SwitchRolesButtonPressed:
                SwitchRolesButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.HouseGrabbed:
                HouseGrabbed += (_) => action.Invoke();
                break;
            case EventPublisherEvents.OpenProjectButtonPressed:
                OpenProjectButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.ViewProjectsButtonPressed:
                i.ViewProjectsButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.HandMenuOpened:
                HandMenuOpened += action.Invoke;
                break;
            case EventPublisherEvents.DesignButtonPressed:
                DesignButtonPressed += (_) => action.Invoke();
                break;
            case EventPublisherEvents.ExitDesignButtonPressed:
                ExitDesignButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.FinalizeDesignButtonPressed:
                FinalizeDesignButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.DesignMenuSliderUpdated:
                DesignMenuSliderUpdated += (_) => action.Invoke();
                break;
            case EventPublisherEvents.BuildingFrameUpdated:
                BuildingFrameUpdated += (_, __) => action.Invoke();
                break;
            case EventPublisherEvents.ManageInventoryButtonPressed:
                ManageInventoryButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.ExitInventoryButtonPressed:
                ExitInventoryButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.ExitOptimizationButtonPressed:
                ExitOptimizationButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.OptimizationSliderUpdated:
                OptimizationSliderUpdated += (_) => action.Invoke();
                break;
            case EventPublisherEvents.CutWoodButtonPressed:
                CutWoodButtonPressed += action.Invoke;
                break;
            case EventPublisherEvents.StartOptimization:
                StartOptimization += action.Invoke;
                break;
            case EventPublisherEvents.CutWoodFinished:
                CutWoodFinished += action.Invoke;
                break;
            default:
                // code block
                break;
        }
    }
    public void OpenSubApp_NONE()
    {
        CallChangePermissions(PermissionLevel.NONE);
        CallViewProjectsButtonPressed();
    }
    public void OpenSubApp_ARCHITECT()
    {
        CallChangePermissions(PermissionLevel.ARCHITECT);
        CallViewProjectsButtonPressed();
    }
    public void OpenSubApp_BUILDER()
    {
        CallChangePermissions(PermissionLevel.BUILDER);
        CallViewProjectsButtonPressed();
    }
    public void OpenSubApp_DEALER_OR_DISTRIBUTOR()
    {
        CallChangePermissions(PermissionLevel.DEALER_OR_DISTRIBUTOR);
        CallViewProjectsButtonPressed();
    }
    public void OpenSubApp_MILLS()
    {
        CallChangePermissions(PermissionLevel.MILLS);
        CallViewProjectsButtonPressed();
    }
    internal void OpenProjectForOptimization(GameObject project)
    {
        projectCurrentlyBeingOptimized = project;
        CallStartOptimization();
    }
}