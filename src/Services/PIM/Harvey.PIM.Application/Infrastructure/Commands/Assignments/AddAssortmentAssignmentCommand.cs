using AutoMapper;
using Harvey.Domain;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Commands.AssortmentAssignments
{
    public class AddAssortmentAssignmentCommand : ICommand<bool>
    {
        public List<AddAssortmentAssignmentModel> Assignments { get; }
        public Guid AssortmentId { get; }

        public AddAssortmentAssignmentCommand(List<AddAssortmentAssignmentModel> assignments, Guid assortmentId)
        {
            Assignments = assignments;
            AssortmentId = assortmentId;
        }
    }

    public class AddAssignmentCommandHandler : ICommandHandler<AddAssortmentAssignmentCommand, bool>
    {
        private readonly IEfRepository<PimDbContext, AssortmentAssignment> _repository;
        public AddAssignmentCommandHandler(IEfRepository<PimDbContext, AssortmentAssignment> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(AddAssortmentAssignmentCommand command)
        {
            var assignments = new List<AssortmentAssignment>();
            var assignmentSelected = await _repository.ListAsync(x => x.AssortmentId == command.AssortmentId);    

            command.Assignments.ForEach(x =>
            {
                var assignmentsSelect = new AssortmentAssignment()
                {
                    AssortmentId = command.AssortmentId,
                    ReferenceId = x.ReferenceId,
                    EntityType = x.EntityType,
                };
                assignments.Add(assignmentsSelect);

            });
            await _repository.DeleteAsync(assignmentSelected.ToList());
            await _repository.AddAsync(assignments);
            return true;
        }
    }
}
