using AxionMaui.Models;
using Newtonsoft.Json;
using System.Text;

namespace AxionMaui;

public partial class LoginPage : ContentPage
{
    private const string ApiUrl = "https://amontefusco-002-site1.ktempurl.com/api/Auth/login";


    public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Obtener los valores de los campos de entrada
        string phoneNumber = phoneNumberEntry.Text; // Asegúrate de que el nombre coincida con el de tu Entry
        string password = passwordEntry.Text; // Asegúrate de que el nombre coincida con el de tu Entry

        // Crear la solicitud
        var loginRequest = new LoginRequest
        {
            PhoneNumber = phoneNumber,
            Password = password
        };

        // Enviar la solicitud a la API
        var responseMessage = await SendLoginRequest(loginRequest);

        // Manejar la respuesta
        if (responseMessage != null)
        {
            if (responseMessage.Message == "Login successful.")
            {
                // Aquí puedes navegar a la página principal (Home)
                await Navigation.PushAsync(new HomePage(responseMessage.FullName));
            }
            else
            {
                // Mostrar un mensaje de error
                await DisplayAlert("Error", responseMessage.Message, "OK");
            }
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Aquí iría la lógica para navegar a la página de registro.
        await Navigation.PushAsync(new RegisterPage());
    }

    private async Task<LoginResponse> SendLoginRequest(LoginRequest loginRequest)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                // Configura el contenido de la solicitud
                var jsonContent = JsonConvert.SerializeObject(loginRequest);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                var response = await httpClient.PostAsync(ApiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);
                }
                else
                {
                    // Manejar respuestas no exitosas (por ejemplo, 401, 400)
                    return new LoginResponse
                    {
                        Message = "Invalid credentials." // Mensaje predeterminado para errores
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes mostrar un mensaje de error)
                await DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }
    }
}