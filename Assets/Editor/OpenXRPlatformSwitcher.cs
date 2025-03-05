using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEditor.SceneManagement;
using UnityEditor.XR.OpenXR.Features;
using UnityEngine.XR.OpenXR;

public class OpenXRPlatformSwitcher
{
    public static readonly string FeatureSetPico = "com.picoxr.openxr.features";
    public static readonly Type[] RequiredFeaturesTypesPico =
    {
        typeof(Unity.XR.OpenXR.Features.PICOSupport.PICOFeature),
        typeof(Unity.XR.OpenXR.Features.PICOSupport.OpenXRExtensions),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.PICO4ControllerProfile),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.PICO4UltraControllerProfile),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.PICOG3ControllerProfile),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.PICONeo3ControllerProfile)
    };

    public static readonly string FeatureSetMeta = "com.unity.openxr.featureset.meta";

    public static readonly Type[] RequiredFeaturesTypesMeta =
    {
        typeof(UnityEngine.XR.OpenXR.Features.MetaQuestSupport.MetaQuestFeature),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.OculusTouchControllerProfile),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.MetaQuestTouchPlusControllerProfile),
        typeof(UnityEngine.XR.OpenXR.Features.Interactions.MetaQuestTouchProControllerProfile)
    };

    [MenuItem("Platforms/Switch to Meta build profile")]
    static void SwitchToMeta()
    {
        ConfigureForMeta();
        OpenFirstScene();
    }

    [MenuItem("Platforms/Switch to Pico build profile")]
    static void SwitchToPico()
    {
        ConfigureForPico();
        OpenFirstScene();
    }

    public static void OpenFirstScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
    }

    public static void ConfigureForPico()
    {
        SetFeatureSetEnabledForOpenXR(FeatureSetMeta, BuildTargetGroup.Android, false);
        SetFeatureSetEnabledForOpenXR(FeatureSetPico, BuildTargetGroup.Android, true);
        SetupFeatures(RequiredFeaturesTypesPico);
    }

    public static void ConfigureForMeta()
    {
        SetFeatureSetEnabledForOpenXR(FeatureSetPico, BuildTargetGroup.Android, false);
        SetFeatureSetEnabledForOpenXR(FeatureSetMeta, BuildTargetGroup.Android, true);
        SetupFeatures(RequiredFeaturesTypesMeta);
    }

    private static void SetupFeatures(Type[] requiredFeatures)
    {
        var settings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);

        var features = settings.GetFeatures();
        foreach (var feature in features)
        {
            if (requiredFeatures.Any(it => it == feature.GetType()))
            {
                if (!feature.enabled)
                    feature.enabled = true;
            }
            else if (feature.enabled)
            {
                feature.enabled = false;
            }
        }
        EditorUtility.SetDirty(settings);
    }

    public static void SetFeatureSetEnabledForOpenXR(
        string XRFeatureSetID,
        BuildTargetGroup buildTargetGroup,
        bool enabled
    )
    {
        OpenXRFeatureSetManager.FeatureSet featureSet = OpenXRFeatureSetManager.GetFeatureSetWithId(
            buildTargetGroup,
            XRFeatureSetID
        );

        if (featureSet == null)
        {
            throw new BuildFailedException("Couldn't find feature set with ID: " + XRFeatureSetID);
        }

        featureSet.isEnabled = enabled;
        OpenXRFeatureSetManager.SetFeaturesFromEnabledFeatureSets(buildTargetGroup);
        AssetDatabase.SaveAssets();
    }

    public static void SwitchToBuildProfile(BuildProfile targetProfile)
    {
        var currentProfile = BuildProfile.GetActiveBuildProfile();
        if (currentProfile == targetProfile)
            return;

        // Don't switch when compiling
        if (EditorApplication.isCompiling)
        {
            throw new BuildFailedException(
                "Could not switch build profile because unity is compiling"
            );
        }

        // Don't switch while playing
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            throw new BuildFailedException(
                "Could not switch build profile because unity is in playMode"
            );
        }

        Log.Info("Switching build profile from {0} to {1}", currentProfile, targetProfile);

        BuildProfile.SetActiveBuildProfile(targetProfile);

        Log.Info("Build profile switched to {0}", targetProfile);
    }
}
