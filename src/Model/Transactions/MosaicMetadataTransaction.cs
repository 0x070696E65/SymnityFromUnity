using System;
using System.Numerics;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;

namespace Symnity.Model.Transactions
{
    public class MosaicMetadataTransaction : Transaction
    {
        /**
         * target account address.
         */
        public readonly UnresolvedAddress TargetAddress;

        /**
         * Metadata key scoped to source, target and type.
         */
        public readonly BigInteger ScopedMetadataKey;

        /**
         * Target mosaic identifier.
         */
        public readonly UnresolvedMosaicId TargetMosaicId;

        /**
         * Change in value size in bytes.
         */
        public readonly short ValueSizeDelta;

        /**
         * String value with UTF-8 encoding.
         * Difference between the previous value and new value.
         */
        public readonly string Value;

        private new string Signature;
        new PublicAccount Signer;
        new TransactionInfo TransactionInfo;

        /**
        * Create a account meta data transaction object
        * @param deadline - transaction deadline
        * @param targetAddress - target account address.
        * @param scopedMetadataKey - Metadata key scoped to source, target and type.
        * @param valueSizeDelta - Change in value size in bytes.
        * @param value - String value with UTF-8 encoding
        *                Difference between the previous value and new value.
        *                You can calculate value as xor(previous-value, new-value).
        *                If there is no previous value, use directly the new value.
        * @param maxFee - (Optional) Max fee defined by the sender
        * @param signature - (Optional) Transaction signature
        * @param signer - (Optional) Signer public account
        * @returns {AccountMetadataTransaction}
        */
        public static MosaicMetadataTransaction Create(
            Deadline deadline,
            UnresolvedAddress targetAddress,
            BigInteger scopedMetadataKey,
            UnresolvedMosaicId targetMosaicId,
            short valueSizeDelta,
            string value,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new MosaicMetadataTransaction(
                networkType,
                TransactionVersion.ACCOUNT_METADATA,
                deadline,
                maxFee,
                targetAddress,
                scopedMetadataKey,
                targetMosaicId,
                valueSizeDelta,
                value,
                signature,
                signer
            );
        }

        /**
        * @param networkType
        * @param version
        * @param deadline
        * @param maxFee
        * @param targetAddress
        * @param scopedMetadataKey
        * @param valueSizeDelta
        * @param value
        * @param signature
        * @param signer
        * @param transactionInfo
        */
        public MosaicMetadataTransaction(
            NetworkType networkType,
            byte version,
            Deadline deadline,
            long maxFee,
            UnresolvedAddress targetAddress, 
            BigInteger scopedMetadataKey, 
            UnresolvedMosaicId targetMosaicId,
            short valueSizeDelta, 
            string value,
            string signature = null,
            PublicAccount signer = null,
            TransactionInfo transactionInfo = null
        )
        : base(TransactionType.ACCOUNT_METADATA, networkType, version, deadline, maxFee, signature, signer,
            transactionInfo)
        {
            TargetAddress = targetAddress;
            ScopedMetadataKey = scopedMetadataKey;
            TargetMosaicId = targetMosaicId;
            ValueSizeDelta = valueSizeDelta;
            Value = value;
        }
        
        /**
        * @internal
        * @returns {byte[]}
        */
        protected override byte[] GenerateBytes()
        {
            return CreateBuilder().Serialize();
        }
        
        /**
         * @description Serialize a transaction object
         * @returns {string}
         * @memberof Transaction
         */
        public override string Serialize()
        {
            return ConvertUtils.ToHex(GenerateBytes());
        }
        
        /**
         * @internal
         * @returns {TransactionBuilder}
         */
        public new MosaicMetadataTransactionBuilder CreateBuilder()
        {
            return new MosaicMetadataTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.ACCOUNT_METADATA),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                new UnresolvedAddressDto(TargetAddress.EncodeUnresolvedAddress()),
                (long)ScopedMetadataKey,
                new UnresolvedMosaicIdDto(TargetMosaicId.GetIdAsLong()),
                ValueSizeDelta,
                ConvertUtils.Utf8ToByteArray(Value)
            );
        }
        
        /**
         * @override Transaction.size()
         * @description get the byte size of a transaction using the builder
         * @returns {number}
         * @memberof TransferTransaction
         */ 
        public override int GetSize() {
            return _payloadSize ?? CreateBuilder().GetSize();
        }
        
        /**
         * Set transaction maxFee using fee multiplier for **ONLY NONE AGGREGATE TRANSACTIONS**
         * @param feeMultiplier The fee multiplier
         * @returns {TransferTransaction}
         */
        public new MosaicMetadataTransaction SetMaxFee(int feeMultiplier)
        {
            if (Type == TransactionType.AGGREGATE_BONDED && Type == TransactionType.AGGREGATE_COMPLETE) {
                throw new Exception("setMaxFee can only be used for none aggregate transactions.");
            }
            MaxFee = feeMultiplier * GetSize();
            return this;
        }
        
        /**
         * Convert an aggregate transaction to an inner transaction including transaction signer.
         * Signer is optional for `AggregateComplete` transaction `ONLY`.
         * If no signer provided, aggregate transaction signer will be delegated on signing
         * @param signer - Innre transaction signer.
         * @returns InnerTransaction
         */
        public MosaicMetadataTransaction ToAggregate(PublicAccount signer)
        {
            Signer = signer;
            return this;
        }
    }
}