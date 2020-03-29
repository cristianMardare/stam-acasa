using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StamAcasa.EmailService.Subscriber;

namespace StamAcasa.EmailService {
	public class Worker : BackgroundService {
		private readonly ISubscriber _subscriber;
		private readonly ILogger<Worker> _logger;

		public Worker (ISubscriber subscriber, ILogger<Worker> logger) {
			_subscriber = subscriber ??
				throw new ArgumentNullException (nameof (subscriber));
			_logger = logger;
		}

		public override Task StartAsync (CancellationToken cancellationToken) {
			_logger.LogInformation ("Email service is starting...");

			_subscriber.Subscribe ("email:requests", (request) => {
				_logger.LogInformation ($"Request to send email to: {request.Address}");

				return Task.CompletedTask;
			});

			return base.StartAsync (cancellationToken);
		}

		protected override async Task ExecuteAsync (CancellationToken stoppingToken) {
			await Task.CompletedTask;
		}

		public override Task StopAsync (CancellationToken cancellationToken) {
			_logger.LogInformation ("Email service is stopping...");

			return base.StopAsync (cancellationToken);

		}

		public override void Dispose () {
			_subscriber.Dispose ();

			base.Dispose ();
		}
	}
}