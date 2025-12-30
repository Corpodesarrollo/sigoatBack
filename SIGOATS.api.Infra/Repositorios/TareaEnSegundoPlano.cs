using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SIGOATS.api.Infra.Repositorios
{
    public class TareaEnSegundoPlano(IServiceProvider serviceProvider, IConfiguration configuration) : BackgroundService
    {
        private Timer _timer;
        private bool _isRunning = false;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var horaEjecucion = TimeSpan.FromHours(2); // Configura la hora de ejecución (2 AM)
            int intervaloHoras = 24;

            // Calcula el tiempo hasta la próxima ejecución a la hora configurada
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, horaEjecucion.Hours, horaEjecucion.Minutes, horaEjecucion.Seconds);
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            var initialDelay = nextRun - now;
            //var initialDelay = now - now;

            // Configura el temporizador para ejecutar la tarea a la hora configurada y luego cada intervalo configurado
            _timer = new Timer(EjecutarTarea, null, initialDelay, TimeSpan.FromHours(intervaloHoras));

            return Task.CompletedTask;
        }

        private async void EjecutarTarea(object state)
        {
            if (_isRunning)
            {
                return; // Si la tarea ya se está ejecutando, no la ejecuta de nuevo
            }

            _isRunning = true;

            try
            {
                await RealizarTareaAsincrona();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la ejecución de la tarea: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
            }
        }

        private async Task RealizarTareaAsincrona()
        {
            using var scope = serviceProvider.CreateScope();

            var usersRepo = scope.ServiceProvider.GetRequiredService<UsersRepo>();
            await usersRepo.BloquearUsuariosInactivos();
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
