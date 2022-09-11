using System;
using HRIS.Domain.JobDescription.Enum;
using Souccar.Domain.DomainModel;

//using HRIS.Domain.Objectives.ValueObjects;

namespace HRIS.Domain.JobDescription.Entities
{
    public class PositionStatus : HistoryEntity
    {
        public PositionStatus()
        {
        }

        public PositionStatus(Position position, PositionStatusType positionStatusType)
        {
            Position = position;
            PositionStatusType = positionStatusType;
            FromDate = DateTime.Now;
        }

        public virtual PositionStatusType PositionStatusType { get; protected set; }
        public virtual Position Position { get; protected set; }
    }
}