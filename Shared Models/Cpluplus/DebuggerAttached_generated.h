// automatically generated by the FlatBuffers compiler, do not modify


#ifndef FLATBUFFERS_GENERATED_DEBUGGERATTACHED_NETMODELS_H_
#define FLATBUFFERS_GENERATED_DEBUGGERATTACHED_NETMODELS_H_

#include "flatbuffers/flatbuffers.h"

// Ensure the included flatbuffers.h is the same version as when this file was
// generated, otherwise it may not be compatible.
static_assert(FLATBUFFERS_VERSION_MAJOR == 23 &&
              FLATBUFFERS_VERSION_MINOR == 5 &&
              FLATBUFFERS_VERSION_REVISION == 26,
             "Non-compatible flatbuffers version included");

namespace NetModels {

struct DebuggerAttached;
struct DebuggerAttachedBuilder;
struct DebuggerAttachedT;

struct DebuggerAttachedT : public ::flatbuffers::NativeTable {
  typedef DebuggerAttached TableType;
  int32_t line_number = 0;
};

struct DebuggerAttached FLATBUFFERS_FINAL_CLASS : private ::flatbuffers::Table {
  typedef DebuggerAttachedT NativeTableType;
  typedef DebuggerAttachedBuilder Builder;
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_LINE_NUMBER = 4
  };
  int32_t line_number() const {
    return GetField<int32_t>(VT_LINE_NUMBER, 0);
  }
  bool Verify(::flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_LINE_NUMBER, 4) &&
           verifier.EndTable();
  }
  DebuggerAttachedT *UnPack(const ::flatbuffers::resolver_function_t *_resolver = nullptr) const;
  void UnPackTo(DebuggerAttachedT *_o, const ::flatbuffers::resolver_function_t *_resolver = nullptr) const;
  static ::flatbuffers::Offset<DebuggerAttached> Pack(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerAttachedT* _o, const ::flatbuffers::rehasher_function_t *_rehasher = nullptr);
};

struct DebuggerAttachedBuilder {
  typedef DebuggerAttached Table;
  ::flatbuffers::FlatBufferBuilder &fbb_;
  ::flatbuffers::uoffset_t start_;
  void add_line_number(int32_t line_number) {
    fbb_.AddElement<int32_t>(DebuggerAttached::VT_LINE_NUMBER, line_number, 0);
  }
  explicit DebuggerAttachedBuilder(::flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  ::flatbuffers::Offset<DebuggerAttached> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = ::flatbuffers::Offset<DebuggerAttached>(end);
    return o;
  }
};

inline ::flatbuffers::Offset<DebuggerAttached> CreateDebuggerAttached(
    ::flatbuffers::FlatBufferBuilder &_fbb,
    int32_t line_number = 0) {
  DebuggerAttachedBuilder builder_(_fbb);
  builder_.add_line_number(line_number);
  return builder_.Finish();
}

::flatbuffers::Offset<DebuggerAttached> CreateDebuggerAttached(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerAttachedT *_o, const ::flatbuffers::rehasher_function_t *_rehasher = nullptr);

inline DebuggerAttachedT *DebuggerAttached::UnPack(const ::flatbuffers::resolver_function_t *_resolver) const {
  auto _o = std::unique_ptr<DebuggerAttachedT>(new DebuggerAttachedT());
  UnPackTo(_o.get(), _resolver);
  return _o.release();
}

inline void DebuggerAttached::UnPackTo(DebuggerAttachedT *_o, const ::flatbuffers::resolver_function_t *_resolver) const {
  (void)_o;
  (void)_resolver;
  { auto _e = line_number(); _o->line_number = _e; }
}

inline ::flatbuffers::Offset<DebuggerAttached> DebuggerAttached::Pack(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerAttachedT* _o, const ::flatbuffers::rehasher_function_t *_rehasher) {
  return CreateDebuggerAttached(_fbb, _o, _rehasher);
}

inline ::flatbuffers::Offset<DebuggerAttached> CreateDebuggerAttached(::flatbuffers::FlatBufferBuilder &_fbb, const DebuggerAttachedT *_o, const ::flatbuffers::rehasher_function_t *_rehasher) {
  (void)_rehasher;
  (void)_o;
  struct _VectorArgs { ::flatbuffers::FlatBufferBuilder *__fbb; const DebuggerAttachedT* __o; const ::flatbuffers::rehasher_function_t *__rehasher; } _va = { &_fbb, _o, _rehasher}; (void)_va;
  auto _line_number = _o->line_number;
  return NetModels::CreateDebuggerAttached(
      _fbb,
      _line_number);
}

inline const NetModels::DebuggerAttached *GetDebuggerAttached(const void *buf) {
  return ::flatbuffers::GetRoot<NetModels::DebuggerAttached>(buf);
}

inline const NetModels::DebuggerAttached *GetSizePrefixedDebuggerAttached(const void *buf) {
  return ::flatbuffers::GetSizePrefixedRoot<NetModels::DebuggerAttached>(buf);
}

inline const char *DebuggerAttachedIdentifier() {
  return "DEBB";
}

inline bool DebuggerAttachedBufferHasIdentifier(const void *buf) {
  return ::flatbuffers::BufferHasIdentifier(
      buf, DebuggerAttachedIdentifier());
}

inline bool SizePrefixedDebuggerAttachedBufferHasIdentifier(const void *buf) {
  return ::flatbuffers::BufferHasIdentifier(
      buf, DebuggerAttachedIdentifier(), true);
}

inline bool VerifyDebuggerAttachedBuffer(
    ::flatbuffers::Verifier &verifier) {
  return verifier.VerifyBuffer<NetModels::DebuggerAttached>(DebuggerAttachedIdentifier());
}

inline bool VerifySizePrefixedDebuggerAttachedBuffer(
    ::flatbuffers::Verifier &verifier) {
  return verifier.VerifySizePrefixedBuffer<NetModels::DebuggerAttached>(DebuggerAttachedIdentifier());
}

inline void FinishDebuggerAttachedBuffer(
    ::flatbuffers::FlatBufferBuilder &fbb,
    ::flatbuffers::Offset<NetModels::DebuggerAttached> root) {
  fbb.Finish(root, DebuggerAttachedIdentifier());
}

inline void FinishSizePrefixedDebuggerAttachedBuffer(
    ::flatbuffers::FlatBufferBuilder &fbb,
    ::flatbuffers::Offset<NetModels::DebuggerAttached> root) {
  fbb.FinishSizePrefixed(root, DebuggerAttachedIdentifier());
}

inline std::unique_ptr<NetModels::DebuggerAttachedT> UnPackDebuggerAttached(
    const void *buf,
    const ::flatbuffers::resolver_function_t *res = nullptr) {
  return std::unique_ptr<NetModels::DebuggerAttachedT>(GetDebuggerAttached(buf)->UnPack(res));
}

inline std::unique_ptr<NetModels::DebuggerAttachedT> UnPackSizePrefixedDebuggerAttached(
    const void *buf,
    const ::flatbuffers::resolver_function_t *res = nullptr) {
  return std::unique_ptr<NetModels::DebuggerAttachedT>(GetSizePrefixedDebuggerAttached(buf)->UnPack(res));
}

}  // namespace NetModels

#endif  // FLATBUFFERS_GENERATED_DEBUGGERATTACHED_NETMODELS_H_
