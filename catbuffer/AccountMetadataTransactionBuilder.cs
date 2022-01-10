/**
*** Copyright (c) 2016-2019, Jaguar0625, gimre, BloodyRookie, Tech Bureau, Corp.
*** Copyright (c) 2020-present, Jaguar0625, gimre, BloodyRookie.
*** All rights reserved.
***
*** This file is part of Catapult.
***
*** Catapult is free software: you can redistribute it and/or modify
*** it under the terms of the GNU Lesser General Public License as published by
*** the Free Software Foundation, either version 3 of the License, or
*** (at your option) any later version.
***
*** Catapult is distributed in the hope that it will be useful,
*** but WITHOUT ANY WARRANTY; without even the implied warranty of
*** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*** GNU Lesser General Public License for more details.
***
*** You should have received a copy of the GNU Lesser General Public License
*** along with Catapult. If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Symbol.Builders {
    /*
    * Associate a key-value state ([metadata](/concepts/metadata.html)) to an **account**.
\note This transaction must **always** be wrapped in an AggregateTransaction so that a cosignature from `target_address` can be provided. Without this cosignature the transaction is invalid.
Compare to MosaicMetadataTransaction and NamespaceMetadataTransaction.
    */
    [Serializable]
    public class AccountMetadataTransactionBuilder: TransactionBuilder {

        /* Account metadata transaction body. */
        public AccountMetadataTransactionBodyBuilder accountMetadataTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountMetadataTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                accountMetadataTransactionBody = AccountMetadataTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountMetadataTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountMetadataTransactionBuilder.
        */
        public new static AccountMetadataTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountMetadataTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param targetAddress Account whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        internal AccountMetadataTransactionBuilder(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedAddressDto targetAddress, long scopedMetadataKey, short valueSizeDelta, byte[] value)
            : base(signature, signerPublicKey, version, network, type, fee, deadline)
        {
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(fee, "fee is null");
            GeneratorUtils.NotNull(deadline, "deadline is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            GeneratorUtils.NotNull(scopedMetadataKey, "scopedMetadataKey is null");
            GeneratorUtils.NotNull(valueSizeDelta, "valueSizeDelta is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.accountMetadataTransactionBody = new AccountMetadataTransactionBodyBuilder(targetAddress, scopedMetadataKey, valueSizeDelta, value);
        }
        
        /*
        * Creates an instance of AccountMetadataTransactionBuilder.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param targetAddress Account whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        * @return Instance of AccountMetadataTransactionBuilder.
        */
        public static  AccountMetadataTransactionBuilder Create(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedAddressDto targetAddress, long scopedMetadataKey, short valueSizeDelta, byte[] value) {
            return new AccountMetadataTransactionBuilder(signature, signerPublicKey, version, network, type, fee, deadline, targetAddress, scopedMetadataKey, valueSizeDelta, value);
        }

        /*
        * Gets Account whose metadata should be modified..
        *
        * @return Account whose metadata should be modified..
        */
        public UnresolvedAddressDto GetTargetAddress() {
            return accountMetadataTransactionBody.GetTargetAddress();
        }

        /*
        * Gets Metadata key scoped to source, target and type..
        *
        * @return Metadata key scoped to source, target and type..
        */
        public long GetScopedMetadataKey() {
            return accountMetadataTransactionBody.GetScopedMetadataKey();
        }

        /*
        * Gets Change in value size in bytes, compared to previous size..
        *
        * @return Change in value size in bytes, compared to previous size..
        */
        public short GetValueSizeDelta() {
            return accountMetadataTransactionBody.GetValueSizeDelta();
        }

        /*
        * Gets Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        *
        * @return Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        public byte[] GetValue() {
            return accountMetadataTransactionBody.GetValue();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += accountMetadataTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new AccountMetadataTransactionBodyBuilder GetBody() {
            return accountMetadataTransactionBody;
        }


    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public new byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var accountMetadataTransactionBodyEntityBytes = (accountMetadataTransactionBody).Serialize();
            bw.Write(accountMetadataTransactionBodyEntityBytes, 0, accountMetadataTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
