var MaskZipCode = IMask(
    document.getElementById('postalcode'),
    {
        mask: [
            {
                mask: '00-000'
            },
            {
                mask: /^\S*?\S*$/
            }
        ]
    });
var maskPhoneNumber = IMask(
    document.getElementById('phone-number'),
    {
        mask: [
            {
                mask: '000 000 000'
            },
            {
                mask: /^\S*?\S*$/
            }
        ]
    });