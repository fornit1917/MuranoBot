using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using MediatR;

namespace MuranoBot.Application.Commands {
	public class UnknownCommand : IRequest<bool> {

		public Destination Destination { get; private set; }
		public string Text { get; private set; }

		public UnknownCommand(Destination destination, string text) {
			Destination = destination;
			Text = text;
		}
	}
}
