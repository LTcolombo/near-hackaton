using utils.injection;

namespace model
{
    [PerNamedObject]
    public class CollectionModel
    {
        private float _data;//todo possibly map enum.type=>float for now only 1 resource

        public void Inc(float value)
        {
            _data += value;
        }

        public void Reset()
        {
            _data = 0;
        }

        public float Get()
        {
            return _data;
        }
    }
}