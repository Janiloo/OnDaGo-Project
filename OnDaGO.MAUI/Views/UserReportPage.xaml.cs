using OnDaGO.MAUI.Models;
using OnDaGO.MAUI.Services;
using System;
using Microsoft.Maui.Controls;
using Refit;
using System.Threading.Tasks;

namespace OnDaGO.MAUI.Views
{
    public partial class UserReportPage : ContentPage
    {
        private readonly ReportService _reportService;
        private string _userId;

        public UserReportPage(ReportService reportService)
        {
            InitializeComponent();
            _reportService = reportService;
            SubmitButton.IsEnabled = false; // Disable the submit button initially
            GetUserProfile();  // Fetch the user profile on page load
        }

        // Method to retrieve the user profile, specifically the UserId
        private async Task GetUserProfile()
        {
            try
            {
                var token = await SecureStorage.GetAsync("jwt_token");

                if (!string.IsNullOrEmpty(token))
                {
                    var client = HttpClientFactory.CreateClient();
                    var authApi = RestService.For<IAuthApi>(client);
                    var response = await authApi.GetUserProfile();

                    if (response != null)
                    {
                        _userId = response.Id;  // Store the UserId for report submission
                        SubmitButton.IsEnabled = true; // Enable the submit button once the user profile is loaded
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user profile: {ex.Message}");
                await DisplayAlert("Error", "Unable to retrieve user profile. Please try again later.", "OK");
            }
        }

        // Event handler for the Submit Report button
        private async void OnSubmitReportClicked(object sender, EventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(SubjectEntry.Text) || string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                await DisplayAlert("Validation Error", "Please enter both subject and description.", "OK");
                return;
            }

            // Ensure we have the UserId before submitting the report
            if (string.IsNullOrEmpty(_userId))
            {
                await DisplayAlert("Error", "Failed to retrieve user information. Please try again.", "OK");
                return;
            }

            // Create a new report with the user's ID and "Pending" status
            var newReport = new ReportItem
            {
                UserId = _userId,  // Include the UserId
                Subject = SubjectEntry.Text,
                Description = DescriptionEditor.Text,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                // Call the API service to create the report
                await _reportService.CreateReportAsync(newReport);
                await DisplayAlert("Success", "Your report has been submitted successfully.", "OK");

                // Optionally, clear the fields or navigate away
                SubjectEntry.Text = string.Empty;
                DescriptionEditor.Text = string.Empty;
            }
            catch (ApiException apiEx)
            {
                // Display detailed API error if available
                await DisplayAlert("Error", $"API error: {apiEx.Content}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while submitting the report: {ex.Message}", "OK");
            }
        }
    }
}
