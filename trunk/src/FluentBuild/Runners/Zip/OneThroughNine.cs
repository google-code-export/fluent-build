namespace FluentBuild.Runners.Zip
{
    public class OneThroughNine
    {
        private readonly ZipCompress _zipCompress;

        public OneThroughNine(ZipCompress zipCompress)
        {
            _zipCompress = zipCompress;
        }

        public ZipCompress One
        {
            get
            {
                _zipCompress.compressionLevel = 1;
                return _zipCompress;
            }
        }

        public ZipCompress Two
        {
            get
            {
                _zipCompress.compressionLevel = 2;
                return _zipCompress;
            }
        }
        public ZipCompress Three
        {
            get
            {
                _zipCompress.compressionLevel = 3;
                return _zipCompress;
            }
        }
        public ZipCompress Four
        {
            get
            {
                _zipCompress.compressionLevel = 4;
                return _zipCompress;
            }
        }
        public ZipCompress Five
        {
            get
            {
                _zipCompress.compressionLevel = 5;
                return _zipCompress;
            }
        }

        public ZipCompress Six
        {
            get
            {
                _zipCompress.compressionLevel = 6;
                return _zipCompress;
            }
        }

        public ZipCompress Seven
        {
            get
            {
                _zipCompress.compressionLevel = 7;
                return _zipCompress;
            }
        }

        public ZipCompress Eight
        {
            get
            {
                _zipCompress.compressionLevel = 8;
                return _zipCompress;
            }
        }

        public ZipCompress Nine
        {
            get
            {
                _zipCompress.compressionLevel = 9;
                return _zipCompress;
            }
        }


    }
}