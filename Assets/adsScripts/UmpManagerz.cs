using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Api;

public class UmpManagerz : MonoBehaviour
{
    ConsentForm _consentForm;
    // Start is called before the first frame update
    void Start()
    {
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                 ""
             }
        };




        if (ConsentInformation.CanRequestAds())
        {
            UnityEngine.Debug.Log("if statement UMP");

            admanager.instance.Initialization();
        }
        else
        {
            UnityEngine.Debug.Log("else staement consent");
            if (ConsentInformation.IsConsentFormAvailable())
            {

                ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
                {
                    if (formError != null)
                    {
                        // Consent gathering failed.
                        UnityEngine.Debug.LogError(formError);
                        return;
                    }

                    // Consent has been gathered.
                    // Here false means users are not under age.
                    ConsentRequestParameters request = new ConsentRequestParameters
                    {
                        TagForUnderAgeOfConsent = false,
                        ConsentDebugSettings = debugSettings,
                    };

                    // Check the current consent information status.
                    ConsentInformation.Update(request, OnConsentInfoUpdated);

                });
            }
            else
            {
                admanager.instance.Initialization();
            }
        }
    }

    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError("Consent Form Error: " + error);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        if (ConsentInformation.CanRequestAds())
        {
            Debug.Log("admanager can request consent initialized");
            admanager.instance.Initialization();
        }
        else
        {
            Debug.Log("can't request consent ");
        }
    }

    void LoadConsentForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }

    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;

        // You are now ready to show the form.
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
    }


    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        // Handle dismissal by reloading form.
        LoadConsentForm();
    }

    // Update is called once per frame


    //ConsentInformation.Reset();
}
