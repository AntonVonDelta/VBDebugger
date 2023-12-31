// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace NetModels
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct StackFrame : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public static StackFrame GetRootAsStackFrame(ByteBuffer _bb) { return GetRootAsStackFrame(_bb, new StackFrame()); }
  public static StackFrame GetRootAsStackFrame(ByteBuffer _bb, StackFrame obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public StackFrame __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public NetModels.SourceCodeReference? Reference { get { int o = __p.__offset(4); return o != 0 ? (NetModels.SourceCodeReference?)(new NetModels.SourceCodeReference()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public NetModels.SourceCodeReference? CurrentInstruction { get { int o = __p.__offset(6); return o != 0 ? (NetModels.SourceCodeReference?)(new NetModels.SourceCodeReference()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public NetModels.Variable? Locals(int j) { int o = __p.__offset(8); return o != 0 ? (NetModels.Variable?)(new NetModels.Variable()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int LocalsLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<NetModels.StackFrame> CreateStackFrame(FlatBufferBuilder builder,
      Offset<NetModels.SourceCodeReference> referenceOffset = default(Offset<NetModels.SourceCodeReference>),
      Offset<NetModels.SourceCodeReference> current_instructionOffset = default(Offset<NetModels.SourceCodeReference>),
      VectorOffset localsOffset = default(VectorOffset)) {
    builder.StartTable(3);
    StackFrame.AddLocals(builder, localsOffset);
    StackFrame.AddCurrentInstruction(builder, current_instructionOffset);
    StackFrame.AddReference(builder, referenceOffset);
    return StackFrame.EndStackFrame(builder);
  }

  public static void StartStackFrame(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddReference(FlatBufferBuilder builder, Offset<NetModels.SourceCodeReference> referenceOffset) { builder.AddOffset(0, referenceOffset.Value, 0); }
  public static void AddCurrentInstruction(FlatBufferBuilder builder, Offset<NetModels.SourceCodeReference> currentInstructionOffset) { builder.AddOffset(1, currentInstructionOffset.Value, 0); }
  public static void AddLocals(FlatBufferBuilder builder, VectorOffset localsOffset) { builder.AddOffset(2, localsOffset.Value, 0); }
  public static VectorOffset CreateLocalsVector(FlatBufferBuilder builder, Offset<NetModels.Variable>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateLocalsVectorBlock(FlatBufferBuilder builder, Offset<NetModels.Variable>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateLocalsVectorBlock(FlatBufferBuilder builder, ArraySegment<Offset<NetModels.Variable>> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateLocalsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<Offset<NetModels.Variable>>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartLocalsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<NetModels.StackFrame> EndStackFrame(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<NetModels.StackFrame>(o);
  }
  public StackFrameT UnPack() {
    var _o = new StackFrameT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(StackFrameT _o) {
    _o.Reference = this.Reference.HasValue ? this.Reference.Value.UnPack() : null;
    _o.CurrentInstruction = this.CurrentInstruction.HasValue ? this.CurrentInstruction.Value.UnPack() : null;
    _o.Locals = new List<NetModels.VariableT>();
    for (var _j = 0; _j < this.LocalsLength; ++_j) {_o.Locals.Add(this.Locals(_j).HasValue ? this.Locals(_j).Value.UnPack() : null);}
  }
  public static Offset<NetModels.StackFrame> Pack(FlatBufferBuilder builder, StackFrameT _o) {
    if (_o == null) return default(Offset<NetModels.StackFrame>);
    var _reference = _o.Reference == null ? default(Offset<NetModels.SourceCodeReference>) : NetModels.SourceCodeReference.Pack(builder, _o.Reference);
    var _current_instruction = _o.CurrentInstruction == null ? default(Offset<NetModels.SourceCodeReference>) : NetModels.SourceCodeReference.Pack(builder, _o.CurrentInstruction);
    var _locals = default(VectorOffset);
    if (_o.Locals != null) {
      var __locals = new Offset<NetModels.Variable>[_o.Locals.Count];
      for (var _j = 0; _j < __locals.Length; ++_j) { __locals[_j] = NetModels.Variable.Pack(builder, _o.Locals[_j]); }
      _locals = CreateLocalsVector(builder, __locals);
    }
    return CreateStackFrame(
      builder,
      _reference,
      _current_instruction,
      _locals);
  }
}

public class StackFrameT
{
  public NetModels.SourceCodeReferenceT Reference { get; set; }
  public NetModels.SourceCodeReferenceT CurrentInstruction { get; set; }
  public List<NetModels.VariableT> Locals { get; set; }

  public StackFrameT() {
    this.Reference = null;
    this.CurrentInstruction = null;
    this.Locals = null;
  }
}


static public class StackFrameVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyTable(tablePos, 4 /*Reference*/, NetModels.SourceCodeReferenceVerify.Verify, false)
      && verifier.VerifyTable(tablePos, 6 /*CurrentInstruction*/, NetModels.SourceCodeReferenceVerify.Verify, false)
      && verifier.VerifyVectorOfTables(tablePos, 8 /*Locals*/, NetModels.VariableVerify.Verify, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
