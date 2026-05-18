import re, os, glob

pipe_dir = 'src/Lycoris.CSRedisCore.Extensions/Services/Pipe'
files = sorted(glob.glob(os.path.join(pipe_dir, 'Redis*Pipe.cs')))

for fp in files:
    if 'RedisCachePipe.cs' in fp:
        continue

    with open(fp, 'r', encoding='utf-8') as f:
        content = f.read()

    # Get interface name from class declaration
    m = re.search(r'class \w+ : (I\w+)', content)
    if not m:
        print(f'SKIP {os.path.basename(fp)}')
        continue
    iface = m.group(1)
    fname = os.path.basename(fp)

    # Remove 3-line comment blocks before sync methods
    comment_pattern = r'[ \t]*/// <summary>\n[ \t]*/// 同步方法，管道模式不支持，请使用异步重载\n[ \t]*/// </summary>\n'
    count = len(re.findall(comment_pattern, content))
    content = re.sub(comment_pattern, '', content)

    # Replace 'public' with explicit interface for throw-methods
    # Pattern: public <returntype> <methodname>(<params>) => throw NewNotSupportedException();
    def replace_pub(m):
        indent = m.group(1)
        ret_type = m.group(2)
        full_sig = m.group(3)
        return f'{indent}{ret_type} {iface}.{full_sig} => throw NewNotSupportedException();'

    throw_pattern = r'(\s+)public (\S+) (\S+\([^)]*\)) => throw NewNotSupportedException\(\);'
    new_count = len(re.findall(throw_pattern, content))
    content = re.sub(throw_pattern, replace_pub, content)

    with open(fp, 'w', encoding='utf-8') as f:
        f.write(content)

    print(f'OK   {fname}: {count} comments removed, {new_count} methods fixed')

# Also handle methods with generic params like <T>(...)
for fp in files:
    if 'RedisCachePipe.cs' in fp:
        continue

    with open(fp, 'r', encoding='utf-8') as f:
        content = f.read()

    m = re.search(r'class \w+ : (I\w+)', content)
    if not m:
        continue
    iface = m.group(1)
    fname = os.path.basename(fp)

    # Pattern: public <returntype> <name><T>(<params>) => throw ... (generic methods)
    def replace_pub_gen(m):
        indent = m.group(1)
        ret_type = m.group(2)
        full_sig = m.group(3)
        return f'{indent}{ret_type} {iface}.{full_sig} => throw NewNotSupportedException();'

    throw_pattern = r'(\s+)public (\S+) (\S+<[^>]+>\([^)]*\)) => throw NewNotSupportedException\(\);'
    gen_count = len(re.findall(throw_pattern, content))
    content = re.sub(throw_pattern, replace_pub_gen, content)

    with open(fp, 'w', encoding='utf-8') as f:
        f.write(content)
    if gen_count > 0:
        print(f'GEN  {fname}: {gen_count} generic methods fixed')

# Also handle where T : class constraints
for fp in files:
    if 'RedisCachePipe.cs' in fp:
        continue

    with open(fp, 'r', encoding='utf-8') as f:
        content = f.read()

    m = re.search(r'class \w+ : (I\w+)', content)
    if not m:
        continue
    iface = m.group(1)
    fname = os.path.basename(fp)

    def replace_pub_where(m):
        indent = m.group(1)
        ret_type = m.group(2)
        full_sig = m.group(3)
        return f'{indent}{ret_type} {iface}.{full_sig} => throw NewNotSupportedException();'

    # Pattern: public <returntype> <name><T>(<params>) where T : class => throw ...
    throw_pattern = r'(\s+)public (\S+) (\S+<T>\([^)]*\) where T : class) => throw NewNotSupportedException\(\);'
    where_count = len(re.findall(throw_pattern, content))
    content = re.sub(throw_pattern, replace_pub_where, content)

    with open(fp, 'w', encoding='utf-8') as f:
        f.write(content)
    if where_count > 0:
        print(f'WH   {fname}: {where_count} where clauses fixed')

print('\nDone!')
