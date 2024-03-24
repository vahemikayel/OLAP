using System;
using System.Collections.Generic;
using System.Text;

namespace GenericRepository.Models
{
    public class BaseEntity<TIdentity>
        where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>//, IConvertible
    {
        private int? _requestedHashCode;
        public virtual TIdentity Id { get; set; }

        public bool IsTransient()
        {
            var isTransient = EqualityComparer<TIdentity>.Default.Equals(Id, default);
            return isTransient;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TIdentity>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (BaseEntity<TIdentity>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return EqualityComparer<TIdentity>.Default.Equals(item.Id, Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }
        public static bool operator ==(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }


        public static bool operator !=(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            return !(left == right);
        }
    }
}
