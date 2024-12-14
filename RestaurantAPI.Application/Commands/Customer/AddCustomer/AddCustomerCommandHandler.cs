using MediatR;
using RestaurantAPI.Domain.DTOs;
using RestaurantAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Commands.Customer.AddCustomer
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, AddCustomerResponse>
    {
        private readonly ICustomerService _customerService;

        public AddCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<AddCustomerResponse> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            // Chama o serviço para adicionar o cliente
            await _customerService.AddAsync(new CustomerDto
            {
                firstName = request.FirstName,
                lastName = request.LastName,
                phoneNumber = request.PhoneNumber
            });

            // Retorna uma resposta com mensagem de sucesso
            return new AddCustomerResponse
            {
                Message = "Customer added successfully.",
                Customer = new CustomerDto
                {
                    firstName = request.FirstName,
                    lastName = request.LastName,
                    phoneNumber = request.PhoneNumber
                }
            };
        }
    }
}
