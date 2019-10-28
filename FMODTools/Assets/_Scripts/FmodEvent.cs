using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class FmodEvent
{
    // Plays oneshot at the position of the object. 

    public static EventInstance PlayOneShot(string eventName, Transform position, Rigidbody rb)
    {
        EventDescription description = RuntimeManager.GetEventDescription(eventName);

        description.createInstance(out EventInstance instance);

        if (string.IsNullOrEmpty(eventName))
            return instance;

        description.is3D(out bool is3D);

        if (is3D)
        {
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position, rb));
            RuntimeManager.AttachInstanceToGameObject(instance, position, rb);
        }

        instance.start();
        instance.release();
        instance.clearHandle();
        return instance;
    }


// Plays a oneshot with a parameter attached. Usefll for footsteps with material. Objects with no rigidbody can set it to null

    public static EventInstance PlayOneShot(string eventName, Transform position, Rigidbody rb,
        PARAMETER_ID parameterId, float value)
    {
        EventDescription description = RuntimeManager.GetEventDescription(eventName);

        description.createInstance(out EventInstance instance);

        if (string.IsNullOrEmpty(eventName))
            return instance;

        description.is3D(out bool is3D);
        instance.setParameterByID(parameterId, value);

        if (is3D)
        {
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position, rb));
            RuntimeManager.AttachInstanceToGameObject(instance, position, rb);
        }

        instance.start();
        instance.release();
        instance.clearHandle();
        return instance;
    }

    // Creates and starts and instance of the sound. Remember to Stop the sound later. Otherwise it will create a memory leak from not being cleared. Use PlayOneShot if the sound aren't looping. Object with no rigidbody can be set to null

    public static EventInstance Play(string eventName, Transform position, Rigidbody rb)
    {
        EventDescription description = RuntimeManager.GetEventDescription(eventName);

        description.createInstance(out EventInstance instance);

        //EventInstance instance = RuntimeManager.CreateInstance(eventName);

        if (string.IsNullOrEmpty(eventName))
            return instance;

        description.is3D(out bool is3D);

        if (is3D)
        {
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position, rb));
            RuntimeManager.AttachInstanceToGameObject(instance, position, rb);
        }

        instance.start();
        return instance;
    }

    //Stops the instance and clears the handle. 
    public static EventInstance Stop(EventInstance instance, FMOD.Studio.STOP_MODE stopMode)
    {
        if (instance.isValid())
        {
            instance.stop(stopMode);
            instance.release();
            instance.clearHandle();
        }
        else
        {
            Debug.LogError("No Instance was found!");
        }

        return instance;
    }

    // Plays a sound on the position of the object. But does not update the position after. 
    public static EventInstance PlayOneShotAtPosition(string eventName, Transform position)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventName);

        if (string.IsNullOrEmpty(eventName))
            return instance;

        FMOD.ATTRIBUTES_3D positionAttribute = RuntimeUtils.To3DAttributes(position);
        instance.set3DAttributes(positionAttribute);

        instance.start();
        instance.release();
        instance.clearHandle();
        return instance;
    }

    // Plays a sound with a parameter on the position of the object. But does not update the position after. 
    public static EventInstance PlayOneShotAtPosition(string eventName, Transform position, PARAMETER_ID parameterId,
        float value)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventName);

        if (string.IsNullOrEmpty(eventName))
            return instance;

        instance.setParameterByID(parameterId, value);

        FMOD.ATTRIBUTES_3D positionAttribute = RuntimeUtils.To3DAttributes(position);
        instance.set3DAttributes(positionAttribute);

        instance.start();
        instance.release();
        instance.clearHandle();
        return instance;
    }


    // Pauses the instance
    public static EventInstance Pause(EventInstance instance)
    {
        if (instance.isValid())
        {
            instance.getPaused(out bool isPaused);
            if (isPaused)
                return instance;

            instance.setPaused(true);
        }
        else
        {
            Debug.LogError("No Instance was found!");
        }

        return instance;
    }


    //Resumes Instance from where it was
    public static EventInstance Resume(EventInstance instance)
    {
        if (instance.isValid())
        {
            instance.getPaused(out bool isPaused);
            if (!isPaused)
                return instance;

            instance.setPaused(false);
        }
        else
        {
            Debug.LogError("No Instance was found!");
        }

        return instance;
    }

    // Useful bool to check if instance is playing.
    public static bool IsPlaying(EventInstance instance)
    {
        if (instance.isValid())
        {
            instance.getPlaybackState(out PLAYBACK_STATE playbackState);
            return (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED);
        }

        return false;
    }


    // Returns the Id of a parameter. More performant for repeated access than using the string method.
    public static PARAMETER_ID GetParameterId(string eventName, string parameterName)
    {
        EventDescription eventDescription = RuntimeManager.GetEventDescription(eventName);
        PARAMETER_DESCRIPTION parameterDescription;

        eventDescription.getParameterDescriptionByName(parameterName, out parameterDescription);
        PARAMETER_ID parameterId = parameterDescription.id;
        return parameterId;
    }

    // Gets the minimum attenuation on the 3D Object.
    public static float GetMinDistance(string eventName)
    {
        EventDescription description = RuntimeManager.GetEventDescription(eventName);

        float minDistance = 0;
        if (!description.isValid())
        {
            Debug.LogError("Invalid Event Name" + eventName);
            return minDistance;
        }

        description.is3D(out bool is3D);
        if (!is3D)
        {
            Debug.LogError(eventName + "Does not have 3D Property set. Are you sure this is a 3D event?");
            return minDistance;
        }

        description.getMinimumDistance(out minDistance);
        return minDistance;
    }

    // Gets the maximum attenuation on the 3D Object.
    public static float GetMaxDistance(string eventName)
    {
        EventDescription description = RuntimeManager.GetEventDescription(eventName);
        float maxDistance = 0;

        if (!description.isValid())
        {
            Debug.LogError("Invalid Event Name" + eventName);
            return maxDistance;
        }

        description.is3D(out bool is3D);
        if (!is3D)
        {
            Debug.LogError(eventName + "Does not have 3D Property set. Are you sure this is a 3D event?");
            return maxDistance;
        }

        description.getMaximumDistance(out maxDistance);
        return maxDistance;
    }

    public static Bus SetBusVolume(string busName, float newValue)
    {
        FMOD.Studio.Bus bus;

        bus = RuntimeManager.GetBus(busName);
        

        bus.setVolume(newValue);

        return bus;
    }

}